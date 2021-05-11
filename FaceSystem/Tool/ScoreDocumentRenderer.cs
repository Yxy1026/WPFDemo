using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;


namespace FaceSystem.Tool
{
    class ScoreDocumentRenderer : IDocumentRenderer
    {

        public void Render(FlowDocument doc, object data)
        {
            TableRowGroup group = doc.FindName("rowsDetails") as TableRowGroup;
            //Style styleCell = doc.Resources["BorderedCell"] as Style;
            // ScoreMaster scoreMaster = new ScoreMaster();
            foreach (StudentScore item in ((List<StudentScore>)data))
            {
                TableRow row = new TableRow();

                //TableCell cell = new TableCell(new Paragraph(new Run(item.Xh)));
                //cell.Style = styleCell;
                //row.Cells.Add(cell);

                TableCell cell = new TableCell(new Paragraph(new Run(item.course_num)));
                //cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run(item.course_name)));
                //cell.Style = styleCell;
                row.Cells.Add(cell);

                //cell = new TableCell(new Paragraph(new Run(item.course_order)));
                ////cell.Style = styleCell;
                //row.Cells.Add(cell);


                cell = new TableCell(new Paragraph(new Run(item.course_credit)));
                //cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run(item.course_score)));
                //cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run(item.course_type)));
                //cell.Style = styleCell;
                row.Cells.Add(cell);

                group.Rows.Add(row);
            }
            TableRowGroup group2 = doc.FindName("studentInfo") as TableRowGroup;
            TableRow row1 = new TableRow();
            TableCell cell1 = new TableCell(new Paragraph(new Run("姓名：" + HttpRequestHelper.studentinfo.XM)));
            row1.Cells.Add(cell1);

            cell1 = new TableCell(new Paragraph(new Run("学号：" + HttpRequestHelper.studentinfo.XH)));
            row1.Cells.Add(cell1);

            cell1 = new TableCell(new Paragraph(new Run("性别：" + HttpRequestHelper.studentinfo.XB)));
            row1.Cells.Add(cell1);

            group2.Rows.Add(row1);
            TableRow row2 = new TableRow();
            cell1 = new TableCell(new Paragraph(new Run("出生日期：" + HttpRequestHelper.studentinfo.CSRQ)));
            row2.Cells.Add(cell1);

            cell1 = new TableCell(new Paragraph(new Run("学院：" + HttpRequestHelper.studentinfo.XSM)));
            row2.Cells.Add(cell1);

            cell1 = new TableCell(new Paragraph(new Run("专业：" + HttpRequestHelper.studentinfo.ZYM)));
            row2.Cells.Add(cell1);

            group2.Rows.Add(row2);
        }
    }
}
