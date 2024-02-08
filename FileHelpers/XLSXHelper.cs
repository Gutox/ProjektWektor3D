using System;
using System.Collections.Generic;
using System.IO;
using IronXL;

namespace ProjektWektor3D.FileHelpers
{
    internal class XLSXHelper : IFileHelper
    {
        public Dictionary<string, Vector3D> ImportFromFile(string path)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension) || !extension.EndsWith("xlsx"))
                throw new Exception("Invalid file path or extension!");

            var vectors = new Dictionary<string, Vector3D>();

            var workbook = WorkBook.Load(path);

            foreach (var sheet in workbook.WorkSheets)
            {
                var range = sheet.GetNamedTable("Vectors");

                if (range == null || range.ColumnCount != 4)
                    throw new Exception("File contains invalid data!");

                if (range.Columns[0].RowCount < 1 || range.Columns[1].RowCount < 1 || range.Columns[2].RowCount < 1 || range.Columns[3].RowCount < 1)
                    throw new Exception("File contains invalid data!");

                if (range.Columns[0].Rows[0].StringValue != "Name" || range.Columns[1].Rows[0].StringValue != "X" || range.Columns[2].Rows[0].StringValue != "Y" ||
                    range.Columns[3].Rows[0].StringValue != "Z")
                    throw new Exception("File contains invalid data!");


                for (var i = 1; i < range.Columns[0].RowCount; i++)
                {
                    var name = range.Columns[0].Rows[i].GetValue<string>();

                    var x = range.Columns[1].Rows[i].GetValue<double>();
                    var y = range.Columns[2].Rows[i].GetValue<double>();
                    var z = range.Columns[3].Rows[i].GetValue<double>();

                    vectors[name] = new Vector3D(x, y, z);
                }
            }

            return vectors;
        }

        public void ExportToFile(string path, Dictionary<string, Vector3D> vectors)
        {
            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension) || !extension.EndsWith("xlsx"))
                throw new Exception("Invalid file path or extension!");

            var workbook = WorkBook.Create();
            var sheet = workbook.DefaultWorkSheet;

            sheet["A1"].Value = "Name";
            sheet["B1"].Value = "X";
            sheet["C1"].Value = "Y";
            sheet["D1"].Value = "Z";

            var row = 2;
            foreach (var vector in vectors)
            {
                sheet[$"A{row}"].Value = vector.Key;
                sheet[$"B{row}"].Value = vector.Value.X;
                sheet[$"C{row}"].Value = vector.Value.Y;
                sheet[$"D{row}"].Value = vector.Value.Z;
                row++;
            }


            sheet.AddNamedTable("Vectors", sheet[$"A1:D{row - 1}"]);

            workbook.SaveAs(path);
        }
    }
}