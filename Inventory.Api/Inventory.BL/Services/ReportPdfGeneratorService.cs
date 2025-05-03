using Inventory.BL.DTOs.ProductDtos;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.BL.Services
{
    public class ReportPdfGeneratorService
    {
        public byte[] GenerateLowStockReport(IEnumerable<ProductDto> products)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header()
                        .Text("Low Stock Report")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn(1);
                            columns.RelativeColumn(1);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("ID").Bold();
                            header.Cell().Text("Name").Bold();
                            header.Cell().Text("Quantity").Bold();
                            header.Cell().Text("Threshold").Bold();
                        });

                        foreach (var product in products)
                        {
                            table.Cell().Text(product.Id.ToString());
                            table.Cell().Text(product.Name);
                            table.Cell().Text(product.Quantity.ToString());
                            table.Cell().Text(product.LowStockThreshold.ToString());
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(txt =>
                        {
                            txt.Span("Generated on: ");
                            txt.Span(DateTime.Now.ToString("dd MMM yyyy")).SemiBold();
                        });
                });
            }).GeneratePdf();
        }
    }
}
