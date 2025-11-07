using ECommerceWebshop.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ECommerceWebshop.Services
{
    public class InvoiceService
    {
        private readonly string _invoicesDirectory;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<InvoiceService> _logger;

        public InvoiceService(IWebHostEnvironment environment, ILogger<InvoiceService> logger)
        {
            _environment = environment;
            _logger = logger;

            try
            {
                // QuestPDF licentie instellen (Community licentie is gratis voor niet-commercieel gebruik)
                QuestPDF.Settings.License = LicenseType.Community;
                _logger.LogInformation("✅ QuestPDF Community License activated");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"⚠️ QuestPDF License warning: {ex.Message}");
            }

            // ✅ FIX: Check if WebRootPath is null or empty
            _logger.LogInformation($"📁 Environment.WebRootPath: {_environment.WebRootPath}");
            _logger.LogInformation($"📁 Environment.ContentRootPath: {_environment.ContentRootPath}");

            if (string.IsNullOrEmpty(_environment.WebRootPath))
            {
                _logger.LogWarning("⚠️ WebRootPath is null or empty! Falling back to wwwroot folder.");
                _invoicesDirectory = Path.Combine(_environment.ContentRootPath, "wwwroot", "invoices");
            }
            else
            {
                _invoicesDirectory = Path.Combine(_environment.WebRootPath, "invoices");
            }

            _logger.LogInformation($"📁 Invoices directory configured: {_invoicesDirectory}");

            // Zorg dat de directory bestaat
            try
            {
                if (!Directory.Exists(_invoicesDirectory))
                {
                    Directory.CreateDirectory(_invoicesDirectory);
                    _logger.LogInformation($"✅ Created invoices directory: {_invoicesDirectory}");
                }
                else
                {
                    _logger.LogInformation($"✅ Invoices directory already exists: {_invoicesDirectory}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Fout bij aanmaken invoices directory: {ex.Message}");
                throw;
            }
        }

        public string GenerateInvoice(Order order)
        {
            try
            {
                _logger.LogInformation($"📄 Starting invoice generation for order {order.OrderNumber}");

                // Validatie
                if (order == null)
                {
                    throw new ArgumentNullException(nameof(order), "Order cannot be null");
                }

                if (order.OrderItems == null || !order.OrderItems.Any())
                {
                    _logger.LogWarning($"⚠️ Order {order.OrderNumber} has no items");
                    throw new InvalidOperationException("Order must have at least one item");
                }

                _logger.LogInformation($"📦 Order has {order.OrderItems.Count} items");

                // Genereer een unieke bestandsnaam
                string fileName = $"Factuur_{order.OrderNumber}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string filePath = Path.Combine(_invoicesDirectory, fileName);

                _logger.LogInformation($"📝 Generating PDF to: {filePath}");

                // Genereer PDF met QuestPDF
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                        page.Header()
                            .Row(row =>
                            {
                                row.RelativeItem().Column(column =>
                                {
                                    column.Item().Text("ECommerce Webshop").FontSize(24).Bold().FontColor(Colors.Blue.Medium);
                                    column.Item().Text("Nieuwstraat 123");
                                    column.Item().Text("1000 Brussel, België");
                                    column.Item().Text("BTW: BE0123.456.789");
                                    column.Item().Text("Tel: +32 (0)48 123 4567");
                                    column.Item().Text("Email: info@shopbe.com");
                                });

                                row.RelativeItem().AlignRight().Column(column =>
                                {
                                    column.Item().AlignRight().Text("FACTUUR").FontSize(28).Bold().FontColor(Colors.Red.Medium);
                                    column.Item().PaddingTop(20);
                                    column.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Cell().AlignRight().Text("Factuurnummer:").Bold();
                                        table.Cell().Text($"INV-{order.OrderNumber}");

                                        table.Cell().AlignRight().Text("Bestelnummer:").Bold();
                                        table.Cell().Text(order.OrderNumber);

                                        table.Cell().AlignRight().Text("Datum:").Bold();
                                        table.Cell().Text(order.OrderDate.ToString("dd-MM-yyyy"));

                                        table.Cell().AlignRight().Text("Status:").Bold();
                                        table.Cell().Text(order.Status);
                                    });
                                });
                            });

                        page.Content()
                            .PaddingVertical(1, Unit.Centimetre)
                            .Column(column =>
                            {
                                // Klantgegevens
                                column.Item().Element(ComposeCustomerInfo);

                                // Producten tabel
                                column.Item().PaddingTop(20).Element(ComposeProductTable);

                                // Totalen
                                column.Item().PaddingTop(10).AlignRight().Element(ComposeTotals);

                                // Betalingsinformatie
                                column.Item().PaddingTop(30).Element(ComposePaymentInfo);
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Grey.Medium));
                                text.Span("Bedankt voor uw bestelling! Voor vragen kunt u contact met ons opnemen via info@shopbe.com\n");
                                text.Span("ECommerce Webshop - Nieuwstraat 123, 1000 Brussel - BTW: BE0123.456.789");
                            });

                        // --- Sous-fonctions locales ---
                        void ComposeCustomerInfo(IContainer container)
                        {
                            container.ShowOnce().Column(column =>
                            {
                                column.Item().Text("KLANTGEGEVENS").FontSize(12).Bold();
                                column.Item().PaddingTop(5);
                                column.Item().Text($"{order.FirstName} {order.LastName}");
                                column.Item().Text(order.Address);
                                column.Item().Text($"{order.PostalCode} {order.City}");
                                column.Item().Text($"Email: {order.Email}");
                                column.Item().Text($"Tel: {order.Phone}");
                            });
                        }

                        void ComposeProductTable(IContainer container)
                        {
                            container.Table(table =>
                            {
                                // Kolom definities
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                });

                                // Header
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Product").Bold();
                                    header.Cell().Element(CellStyle).AlignRight().Text("Prijs").Bold();
                                    header.Cell().Element(CellStyle).AlignCenter().Text("Aantal").Bold();
                                    header.Cell().Element(CellStyle).AlignRight().Text("Subtotaal").Bold();

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                // Producten
                                foreach (var item in order.OrderItems)
                                {
                                    table.Cell().Element(CellStyle).Text(item.ProductName);
                                    table.Cell().Element(CellStyle).AlignRight().Text($"€ {item.Price:F2}");
                                    table.Cell().Element(CellStyle).AlignCenter().Text(item.Quantity.ToString());
                                    table.Cell().Element(CellStyle).AlignRight().Text($"€ {item.Total:F2}");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                    }
                                }
                            });
                        }

                        void ComposeTotals(IContainer container)
                        {
                            container.Column(column =>
                            {
                                column.Item().Width(200).Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn();
                                        columns.RelativeColumn();
                                    });

                                    table.Cell().AlignRight().Text("Subtotaal:");
                                    table.Cell().AlignRight().Text($"€ {order.TotalAmount:F2}");

                                    table.Cell().AlignRight().Text("Verzendkosten:");
                                    table.Cell().AlignRight().Text("€ 0,00");

                                    table.Cell().AlignRight().Text("BTW (21%):");
                                    table.Cell().AlignRight().Text($"€ {order.TotalAmount * 0.21m:F2}");

                                    table.Cell().AlignRight().Text("TOTAAL:").Bold().FontSize(12);
                                    table.Cell().AlignRight().Text($"€ {order.TotalAmount:F2}").Bold().FontSize(12).FontColor(Colors.Blue.Medium);
                                });
                            });
                        }

                        void ComposePaymentInfo(IContainer container)
                        {
                            container.Column(column =>
                            {
                                column.Item().Text("BETALINGSINFORMATIE").FontSize(11).Bold();
                                column.Item().PaddingTop(5).Text("Bank: BNP Paribas Fortis").FontSize(9);
                                column.Item().Text("IBAN: BE68 5390 0754 7034").FontSize(9);
                                column.Item().Text("BIC: GKCCBEBB").FontSize(9);
                                column.Item().Text($"Mededeling: {order.OrderNumber}").FontSize(9);
                            });
                        }
                    });
                })
                .GeneratePdf(filePath);

                _logger.LogInformation($"✅ Invoice generated successfully: {fileName}");
                _logger.LogInformation($"📁 Full path: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Fout bij genereren factuur: {ex.Message}");
                _logger.LogError($"Stack trace: {ex.StackTrace}");
                throw new Exception($"Fout bij genereren factuur: {ex.Message}", ex);
            }
        }

        public string GetInvoicePath(string orderNumber)
        {
            try
            {
                var files = Directory.GetFiles(_invoicesDirectory, $"Factuur_{orderNumber}_*.pdf");
                if (files.Length > 0)
                {
                    _logger.LogInformation($"✅ Found invoice for order {orderNumber}");
                    return files[0];
                }
                else
                {
                    _logger.LogWarning($"⚠️ No invoice found for order {orderNumber}");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error getting invoice path: {ex.Message}");
                return string.Empty;
            }
        }

        public bool InvoiceExists(string orderNumber)
        {
            try
            {
                var files = Directory.GetFiles(_invoicesDirectory, $"Factuur_{orderNumber}_*.pdf");
                bool exists = files.Length > 0;
                _logger.LogInformation($"Invoice exists for {orderNumber}: {exists}");
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error checking invoice existence: {ex.Message}");
                return false;
            }
        }

        public string GetInvoiceFileName(string orderNumber)
        {
            try
            {
                var filePath = GetInvoicePath(orderNumber);
                return !string.IsNullOrEmpty(filePath) ? Path.GetFileName(filePath) : string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError($"❌ Error getting invoice filename: {ex.Message}");
                return string.Empty;
            }
        }
    }
}