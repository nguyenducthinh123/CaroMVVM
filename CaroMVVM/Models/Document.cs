using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    partial class Document
    {
        public string Name { get { return GetString(nameof(Name)); } set => Push(nameof(Name), value); }
        public int Size { get { return GetValue<int>(nameof(Size)); } set => Push(nameof(Size), value); }
        public int Row { get { return GetValue<int>(nameof(Row)); } set => Push(nameof(Row), value); }
        public int Column { get { return GetValue<int>(nameof(Column)); } set => Push(nameof(Column), value); }
        public char Icon { get { return GetValue<char>(nameof(Icon)); } set => Push(nameof(Icon), value); }
        public string CColor { get { return GetString(nameof(CColor)); } set => Push(nameof(CColor), value); }
        public string XColor { get { return GetString(nameof(XColor)); } set => Push(nameof(XColor), value); }
        public string OColor { get { return GetString(nameof(OColor)); } set => Push(nameof(OColor), value); }
        public int CellSize { get {  return GetValue<int>(nameof(CellSize));} set => Push(nameof(CellSize), value); }
        public bool IsWin { get {  return GetValue<bool>(nameof(IsWin));} set => Push(nameof(IsWin), value); }
    }
}
