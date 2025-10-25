using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using G3MWL.Models;

namespace G3MWL.Documents
{
    public class PatientReportDocument : IDocument
    {
        private readonly Patient _patient;

        public PatientReportDocument(Patient patient)
        {
            _patient = patient;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(40);
                page.Header().Text("Patient Report").FontSize(20).Bold().AlignCenter();

                page.Content().Column(col =>
                {
                    col.Item().Text($"Full Name: {_patient.FirstName} {_patient.LastName}");
                    col.Item().Text($"ID: {_patient._id}");
                    col.Item().Text($"Age: {_patient.Age}");
                    col.Item().Text($"Gender: {_patient.Gender}");
                    col.Item().Text($"Treatment Start Date: {_patient.TreatmentStartDate:dd MMM yyyy}");
                    col.Item().Text($"Starting Weight: {_patient.StartingWeight} kg");

                    if (_patient.WeeklyReports?.Any() == true)
                    {
                        col.Item().PaddingTop(10).Text("Weekly Reports").FontSize(16).Bold();

                        foreach (var report in _patient.WeeklyReports)
                        {
                            col.Item().Text($"Week {report.WeekNumber}").Bold();
                            col.Item().Text($"Weight: {(report.Weight.HasValue ? $"{report.Weight} kg" : "N/A")}");
                            col.Item().Text($"Dosage: {report.Dosage}");
                            col.Item().Text($"Side Effects: {(report.SideEffects.Any() ? string.Join(", ", report.SideEffects) : "None")}");
                            col.Item().Text($"Results: {report.Results}");
                            col.Item().Text($"Notes: {report.Notes}");
                            col.Item().Text("");
                        }
                    }
                });

                page.Footer().AlignCenter().Text($"Generated on {DateTime.Now:dd MMM yyyy HH:mm}");
            });
        }
    }
}

