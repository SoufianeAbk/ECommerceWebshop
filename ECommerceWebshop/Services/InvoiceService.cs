using ECommerceWebshop.Models;
using System.Diagnostics;

namespace ECommerceWebshop.Services
{
    public class InvoiceService
    {
        private readonly string _invoicesDirectory;

        public InvoiceService()
        {
            // Invoices worden opgeslagen in /mnt/user-data/outputs/Invoices
            _invoicesDirectory = "/mnt/user-data/outputs/Invoices";

            // Zorg dat de directory bestaat
            if (!Directory.Exists(_invoicesDirectory))
            {
                Directory.CreateDirectory(_invoicesDirectory);
            }
        }

        public string GenerateInvoice(Order order)
        {
            // Genereer een unieke bestandsnaam
            string fileName = $"Factuur_{order.OrderNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(_invoicesDirectory, fileName);

            // Python script pad
            string pythonScript = CreatePythonScript(order, filePath);
            string scriptPath = Path.Combine(Path.GetTempPath(), $"invoice_{Guid.NewGuid()}.py");
            File.WriteAllText(scriptPath, pythonScript);

            try
            {
                // Voer Python script uit
                var processInfo = new ProcessStartInfo
                {
                    FileName = "python3",
                    Arguments = scriptPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(processInfo))
                {
                    process?.WaitForExit();
                    string output = process?.StandardOutput.ReadToEnd() ?? "";
                    string error = process?.StandardError.ReadToEnd() ?? "";

                    if (process?.ExitCode != 0)
                    {
                        throw new Exception($"PDF generation failed: {error}");
                    }
                }

                return filePath;
            }
            finally
            {
                // Opruimen van tijdelijk script
                if (File.Exists(scriptPath))
                {
                    File.Delete(scriptPath);
                }
            }
        }

        private string CreatePythonScript(Order order, string outputPath)
        {
            // Escape strings voor Python
            string EscapeString(string? str) => str?.Replace("\\", "\\\\").Replace("\"", "\\\"") ?? "";

            var script = $@"
#!/usr/bin/env python3
import subprocess
import sys

# Installeer reportlab als het niet bestaat
try:
    from reportlab.lib.pagesizes import A4
    from reportlab.lib import colors
    from reportlab.lib.units import cm
    from reportlab.pdfgen import canvas
    from reportlab.platypus import Table, TableStyle
except ImportError:
    print('Installing reportlab...', file=sys.stderr)
    subprocess.check_call([sys.executable, '-m', 'pip', 'install', 'reportlab', '--break-system-packages'])
    from reportlab.lib.pagesizes import A4
    from reportlab.lib import colors
    from reportlab.lib.units import cm
    from reportlab.pdfgen import canvas
    from reportlab.platypus import Table, TableStyle

from datetime import datetime

def create_invoice():
    # Maak PDF
    c = canvas.Canvas(r'{EscapeString(outputPath)}', pagesize=A4)
    width, height = A4
    
    # Marges
    margin_left = 2 * cm
    margin_right = width - 2 * cm
    
    # Header - Bedrijfsinformatie
    c.setFont('Helvetica-Bold', 24)
    c.setFillColorRGB(0.17, 0.24, 0.31)  # #2c3e50
    c.drawString(margin_left, height - 2*cm, 'ECommerce Webshop')
    
    # Logo emoji (decoratief)
    c.setFont('Helvetica', 40)
    c.drawString(margin_right - 1.5*cm, height - 2.5*cm, '🛒')
    
    # Bedrijfsdetails
    c.setFont('Helvetica', 10)
    c.setFillColorRGB(0, 0, 0)
    y_pos = height - 3*cm
    c.drawString(margin_left, y_pos, 'Nieuwstraat 123')
    c.drawString(margin_left, y_pos - 0.5*cm, '1000 Brussel, België')
    c.drawString(margin_left, y_pos - 1*cm, 'BTW: BE0123.456.789')
    c.drawString(margin_left, y_pos - 1.5*cm, 'Tel: +32 (0)48 123 4567')
    c.drawString(margin_left, y_pos - 2*cm, 'Email: info@shopbe.com')
    
    # FACTUUR titel
    c.setFont('Helvetica-Bold', 28)
    c.setFillColorRGB(0.91, 0.30, 0.24)  # #e74c3c
    c.drawString(margin_right - 4*cm, height - 5*cm, 'FACTUUR')
    
    # Factuurnummer en datum
    c.setFont('Helvetica-Bold', 11)
    c.setFillColorRGB(0, 0, 0)
    c.drawString(margin_right - 5*cm, height - 6*cm, 'Factuurnummer:')
    c.drawString(margin_right - 5*cm, height - 6.6*cm, 'Bestelnummer:')
    c.drawString(margin_right - 5*cm, height - 7.2*cm, 'Datum:')
    c.drawString(margin_right - 5*cm, height - 7.8*cm, 'Status:')
    
    c.setFont('Helvetica', 11)
    c.drawString(margin_right - 5*cm + 3.5*cm, height - 6*cm, 'INV-{EscapeString(order.OrderNumber)}')
    c.drawString(margin_right - 5*cm + 3.5*cm, height - 6.6*cm, '{EscapeString(order.OrderNumber)}')
    c.drawString(margin_right - 5*cm + 3.5*cm, height - 7.2*cm, '{order.OrderDate:dd-MM-yyyy}')
    c.drawString(margin_right - 5*cm + 3.5*cm, height - 7.8*cm, '{EscapeString(order.Status)}')
    
    # Klantinformatie
    c.setFont('Helvetica-Bold', 12)
    y_pos = height - 9*cm
    c.drawString(margin_left, y_pos, 'KLANTGEGEVENS')
    
    c.setFont('Helvetica', 10)
    y_pos -= 0.7*cm
    c.drawString(margin_left, y_pos, '{EscapeString(order.FirstName)} {EscapeString(order.LastName)}')
    y_pos -= 0.5*cm
    c.drawString(margin_left, y_pos, '{EscapeString(order.Address)}')
    y_pos -= 0.5*cm
    c.drawString(margin_left, y_pos, '{EscapeString(order.PostalCode)} {EscapeString(order.City)}')
    y_pos -= 0.5*cm
    c.drawString(margin_left, y_pos, 'Email: {EscapeString(order.Email)}')
    y_pos -= 0.5*cm
    c.drawString(margin_left, y_pos, 'Tel: {EscapeString(order.Phone)}')
    
    # Producten tabel
    y_pos = height - 14*cm
    
    # Tabel data voorbereiden
    data = [['Product', 'Prijs', 'Aantal', 'Subtotaal']]
    
    # Voeg producten toe
{CreateOrderItemsScript(order)}
    
    # Voeg totalen toe
    data.append(['', '', '', ''])
    data.append(['', '', 'Subtotaal:', '€ {order.TotalAmount:F2}'])
    data.append(['', '', 'Verzendkosten:', '€ 0,00'])
    data.append(['', '', 'BTW (21%):', '€ {order.TotalAmount * 0.21m:F2}'])
    data.append(['', '', 'TOTAAL:', '€ {order.TotalAmount:F2}'])
    
    # Maak tabel
    table = Table(data, colWidths=[10*cm, 2.5*cm, 2*cm, 2.5*cm])
    
    # Stijl van tabel
    table.setStyle(TableStyle([
        # Header
        ('BACKGROUND', (0, 0), (-1, 0), colors.HexColor('#2c3e50')),
        ('TEXTCOLOR', (0, 0), (-1, 0), colors.whitesmoke),
        ('ALIGN', (0, 0), (-1, 0), 'CENTER'),
        ('FONTNAME', (0, 0), (-1, 0), 'Helvetica-Bold'),
        ('FONTSIZE', (0, 0), (-1, 0), 11),
        ('BOTTOMPADDING', (0, 0), (-1, 0), 12),
        ('TOPPADDING', (0, 0), (-1, 0), 12),
        
        # Body
        ('BACKGROUND', (0, 1), (-1, -5), colors.beige),
        ('TEXTCOLOR', (0, 1), (-1, -1), colors.black),
        ('ALIGN', (1, 1), (-1, -1), 'RIGHT'),
        ('ALIGN', (0, 1), (0, -1), 'LEFT'),
        ('FONTNAME', (0, 1), (-1, -1), 'Helvetica'),
        ('FONTSIZE', (0, 1), (-1, -1), 10),
        ('ROWBACKGROUNDS', (0, 1), (-1, -5), [colors.white, colors.lightgrey]),
        
        # Totalen
        ('FONTNAME', (2, -4), (-1, -1), 'Helvetica-Bold'),
        ('BACKGROUND', (0, -1), (-1, -1), colors.HexColor('#e74c3c')),
        ('TEXTCOLOR', (0, -1), (-1, -1), colors.whitesmoke),
        ('FONTSIZE', (0, -1), (-1, -1), 12),
        
        # Grid
        ('GRID', (0, 0), (-1, -5), 1, colors.grey),
        ('LINEBELOW', (2, -4), (-1, -2), 1, colors.grey),
        ('LINEABOVE', (2, -1), (-1, -1), 2, colors.black),
        
        # Padding
        ('TOPPADDING', (0, 1), (-1, -1), 8),
        ('BOTTOMPADDING', (0, 1), (-1, -1), 8),
        ('LEFTPADDING', (0, 0), (-1, -1), 10),
        ('RIGHTPADDING', (0, 0), (-1, -1), 10),
    ]))
    
    # Teken tabel
    table.wrapOn(c, width, height)
    table.drawOn(c, margin_left, y_pos - len(data) * 0.7*cm)
    
    # Footer - Betalingsinformatie
    y_pos = 5*cm
    c.setFont('Helvetica-Bold', 11)
    c.drawString(margin_left, y_pos, 'BETALINGSINFORMATIE')
    
    c.setFont('Helvetica', 9)
    y_pos -= 0.6*cm
    c.drawString(margin_left, y_pos, 'Bank: BNP Paribas Fortis')
    y_pos -= 0.4*cm
    c.drawString(margin_left, y_pos, 'IBAN: BE68 5390 0754 7034')
    y_pos -= 0.4*cm
    c.drawString(margin_left, y_pos, 'BIC: GKCCBEBB')
    y_pos -= 0.4*cm
    c.drawString(margin_left, y_pos, f'Mededeling: {EscapeString(order.OrderNumber)}')
    
    # Footer tekst
    y_pos = 2.5*cm
    c.setFont('Helvetica', 8)
    c.setFillColorRGB(0.5, 0.5, 0.5)
    footer_text = 'Bedankt voor uw bestelling! Voor vragen kunt u contact met ons opnemen via info@shopbe.com of +32 (0)48 123 4567'
    c.drawCentredString(width / 2, y_pos, footer_text)
    
    y_pos -= 0.5*cm
    c.drawCentredString(width / 2, y_pos, 'ECommerce Webshop - Nieuwstraat 123, 1000 Brussel - BTW: BE0123.456.789')
    
    # Lijn onder footer
    c.setStrokeColorRGB(0.17, 0.24, 0.31)
    c.line(margin_left, 3*cm, margin_right, 3*cm)
    
    # Sla op
    c.save()
    print(f'Factuur succesvol gegenereerd: {{outputPath}}')

if __name__ == '__main__':
    create_invoice()
";

            return script;
        }

        private string CreateOrderItemsScript(Order order)
        {
            var items = new System.Text.StringBuilder();
            foreach (var item in order.OrderItems)
            {
                string productName = item.ProductName.Replace("\\", "\\\\").Replace("'", "\\'");
                items.AppendLine($"    data.append(['{productName}', '€ {item.Price:F2}', '{item.Quantity}', '€ {item.Total:F2}'])");
            }
            return items.ToString();
        }

        public string GetInvoicePath(string orderNumber)
        {
            // Zoek de factuur op basis van bestelnummer
            var files = Directory.GetFiles(_invoicesDirectory, $"Factuur_{orderNumber}_*.pdf");
            return files.Length > 0 ? files[0] : string.Empty;
        }

        public bool InvoiceExists(string orderNumber)
        {
            var files = Directory.GetFiles(_invoicesDirectory, $"Factuur_{orderNumber}_*.pdf");
            return files.Length > 0;
        }
    }
}