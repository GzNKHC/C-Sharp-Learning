using System;
using System.Data;
using System.IO;
using System.Linq;
using ReportDocument = CrystalDecisions.CrystalReports.Engine.ReportDocument;
using CrystalDecisions.ReportAppServer.ClientDoc;
using CrystalDecisions.ReportAppServer.Controllers;
using CrystalDecisions.ReportAppServer.DataDefModel;
using CrystalDecisions.ReportAppServer.DataSetConversion;
using CrystalDecisions.ReportAppServer.ReportDefModel;
using CrystalDecisions.Shared;
using RptSection = CrystalDecisions.ReportAppServer.ReportDefModel.Section;

namespace ThucHanh8
{
    internal static class StudentReportTemplate
    {
        public static ReportDocument Create(DataTable rows)
        {
            var report = new ReportDocument();
            try
            {
                string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                string template = Path.Combine(directory, "MyReport.rpt");
                if (File.Exists(template))
                {
                    report.Load(template, OpenReportMethod.OpenReportByTempCopy);
                    report.SetDataSource(rows);
                }
                else
                {
                    report.Load(Path.Combine(directory, "BaseReport.rpt"), OpenReportMethod.OpenReportByTempCopy);
                    BuildLayout(report, rows);
                }
                return report;
            }
            catch
            {
                report.Dispose();
                throw;
            }
        }

        public static void Save(ReportDocument report, string path)
        {
            report.ReportOptions.EnableSaveDataWithReport = false;
            object directory = Path.GetDirectoryName(path);
            report.ReportClientDocument.SaveAs(Path.GetFileName(path), ref directory,
                (int)CdReportClientDocumentSaveAsOptionsEnum.cdReportClientDocumentSaveAsOverwriteExisting);
        }

        private static void BuildLayout(ReportDocument report, DataTable rows)
        {
            ISCDReportClientDocument model = report.ReportClientDocument;
            var layout = model.ReportDefController;
            var database = model.DatabaseController;

            report.RecordSelectionFormula = "";
            foreach (ISCRArea area in layout.ReportDefinition.Areas)
                foreach (RptSection section in area.Sections)
                    RemoveObjects(layout, section);

            for (int index = model.DataDefinition.Groups.Count - 1; index >= 0; index--)
                model.DataDefController.GroupController.Remove(index);
            for (int index = model.DataDefinition.Sorts.Count - 1; index >= 0; index--)
                model.DataDefController.SortController.Remove(index);
            for (int index = database.Database.Tables.Count - 1; index >= 0; index--)
                database.RemoveTable(database.Database.Tables[index].Alias);

            var dataset = new System.Data.DataSet("BaoCaoSinhVien");
            dataset.Tables.Add(rows.Copy());
            database.AddDataSource(DataSetConverter.Convert(dataset));
            ISCRTable table = database.Database.Tables.FindTableByAlias(StudentReportData.TableName);
            foreach (ISCRField field in table.DataFields)
                if (model.DataDefinition.ResultFields.FindIndexOf(field) < 0)
                    model.DataDefController.ResultFieldController.Add(-1, field);

            ISCRField classField = Find(table, "MaLop");
            int groupIndex = model.DataDefController.GroupController.Add(-1, new Group { ConditionField = classField });
            model.DataDefController.SortController.Add(-1, new Sort
            {
                SortField = Find(table, "MaSinhVien"),
                Direction = CrSortDirectionEnum.crSortDirectionAscendingOrder
            });

            report.PrintOptions.PaperSize = PaperSize.PaperA4;
            report.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
            model.PrintOutputController.ModifyPageMargins(690, 690, 740, 740);
            report.ReportOptions.EnableSaveDataWithReport = false;

            var definition = layout.ReportDefinition;
            RptSection page = definition.PageHeaderArea.Sections[0];
            ISCRArea groupArea = definition.GroupHeaderArea[groupIndex];
            var groupFormat = (ISCRGroupAreaFormat)groupArea.Format.Clone(true);
            groupFormat.EnableRepeatGroupHeader = true;
            layout.ReportAreaController.SetProperty(groupArea,
                CrReportAreaPropertyEnum.crReportAreaPropertyFormat, groupFormat);
            RptSection groupHeader = groupArea.Sections[0];
            RptSection detail = definition.DetailArea.Sections[0];
            RptSection groupFooter = definition.GroupFooterArea[groupIndex].Sections[0];
            RemoveObjects(layout, groupHeader);

            SetHeight(layout, definition.ReportHeaderArea.Sections[0], 0);
            SetHeight(layout, definition.ReportFooterArea.Sections[0], 0);
            SetHeight(layout, definition.PageFooterArea.Sections[0], 0);
            SetHeight(layout, page, 660);
            SetHeight(layout, groupHeader, 760);
            SetHeight(layout, detail, 400);
            SetHeight(layout, groupFooter, 720);

            const CrAlignmentEnum center = CrAlignmentEnum.crAlignmentHorizontalCenter;
            const CrAlignmentEnum left = CrAlignmentEnum.crAlignmentLeft;
            AddText(layout, page, "ReportTitle", "DANH SÁCH SINH VIÊN", 0, 70, 10400, 480, 18, true, center);

            AddText(layout, groupHeader, "LabelLop", "Lớp", 120, 60, 690, 300, 11, true, left);
            AddField(layout, groupHeader, Find(table, "Lop"), 850, 60, 9500, 300, true, left);
            AddLine(layout, groupHeader, "HeaderRule", 120, 405, 10340);

            AddText(layout, groupHeader, "LabelSTT", "STT", 120, 460, 910, 270, 10, true, center);
            AddText(layout, groupHeader, "LabelMa", "Mã sinh viên", 1280, 460, 2240, 270, 10, true, left);
            AddText(layout, groupHeader, "LabelHoTen", "Họ tên", 3700, 460, 4350, 270, 10, true, left);
            AddText(layout, groupHeader, "LabelNgaySinh", "Ngày sinh", 8220, 460, 2100, 270, 10, true, center);

            AddField(layout, detail, Find(table, "STT"), 120, 30, 910, 320, false, center);
            AddField(layout, detail, Find(table, "MaSinhVien"), 1280, 30, 2240, 320, false, left);
            AddField(layout, detail, Find(table, "HoTen"), 3700, 30, 4350, 320, false, left);
            AddField(layout, detail, Find(table, "NgaySinh"), 8220, 30, 2100, 320, false, center);

            AddLine(layout, groupFooter, "TotalRule", 6980, 70, 10340);
            AddText(layout, groupFooter, "LabelTongSo", "Tổng số sinh viên", 6980, 130, 2450, 330, 10, true, left);
            AddField(layout, groupFooter, Find(table, "TongSoTheoLop"), 9450, 130, 880, 330, true, center);
        }

        private static ISCRField Find(ISCRTable table, string name)
        {
            return table.DataFields.Cast<ISCRField>().Single(field => field.Name == name);
        }

        private static void RemoveObjects(ReportDefController2 layout, RptSection section)
        {
            foreach (ISCRReportObject item in section.ReportObjects.Cast<ISCRReportObject>().ToArray())
                layout.ReportObjectController.Remove(item);
        }

        private static void SetHeight(ReportDefController2 layout, RptSection section, int height)
        {
            if (height > 0 && section.Format.EnableSuppress)
            {
                var format = section.Format.Clone(true);
                format.EnableSuppress = false;
                layout.ReportSectionController.SetProperty(section,
                    CrReportSectionPropertyEnum.crReportSectionPropertyFormat, format);
            }
            layout.ReportSectionController.SetProperty(section,
                CrReportSectionPropertyEnum.crReportSectionPropertyHeight, height);
        }

        private static FontColor Typeface(short size, bool bold)
        {
            return new FontColor
            {
                Color = 0,
                Font = new CrystalDecisions.ReportAppServer.ReportDefModel.Font
                {
                    Name = "Times New Roman", Size = size, Bold = bold
                }
            };
        }

        private static void AddText(ReportDefController2 layout, RptSection section, string name, string text,
            int x, int y, int width, int height, short size, bool bold, CrAlignmentEnum alignment)
        {
            FontColor font = Typeface(size, bold);
            var element = new ParagraphTextElement { Text = text, FontColor = font };
            var paragraph = new Paragraph { Alignment = alignment };
            paragraph.ParagraphElements.Add(element);
            var item = new CrystalDecisions.ReportAppServer.ReportDefModel.TextObject
            {
                Name = name, Left = x, Top = y, Width = width, Height = height, FontColor = font
            };
            item.Paragraphs.Add(paragraph);
            item.Format.HorizontalAlignment = alignment;
            layout.ReportObjectController.Add(item, section, -1);
        }

        private static void AddField(ReportDefController2 layout, RptSection section, ISCRField field,
            int x, int y, int width, int height, bool bold, CrAlignmentEnum alignment)
        {
            var item = new CrystalDecisions.ReportAppServer.ReportDefModel.FieldObject
            {
                Name = "Rpt" + field.Name,
                Left = x, Top = y, Width = width, Height = height,
                DataSourceName = field.FormulaForm, FieldValueType = field.Type,
                FontColor = Typeface(11, bold)
            };
            item.Format.HorizontalAlignment = alignment;
            item.Format.EnableCanGrow = field.Name == "HoTen" || field.Name == "Lop";
            layout.ReportObjectController.Add(item, section, -1);
        }

        private static void AddLine(ReportDefController2 layout, RptSection section, string name,
            int left, int top, int right)
        {
            var line = new CrystalDecisions.ReportAppServer.ReportDefModel.LineObject
            {
                Name = name, Left = left, Top = top, Right = right, Bottom = top,
                LineStyle = CrLineStyleEnum.crLineStyleSingle
            };
            layout.ReportObjectController.Add(line, section, -1);
        }
    }
}
