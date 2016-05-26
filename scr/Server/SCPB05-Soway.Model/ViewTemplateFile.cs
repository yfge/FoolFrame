using Soway.Data.Discription.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Soway.Model.View
{
    [DataContract(IsReference = true)]
    [Table(Name = "SW_SYS_VIEW_FILE",ColPreStr ="VIEW_FILE_")]
    public class ViewTemplateFile
    {
        [Column(ColumnName = "NAME",IsKey =true,KeyGroupName ="Name")]
        public string Name
        {
            get; set;
        }
        [Column(ColumnName ="ID",IsKey = true,IsAutoGenerate = GenerationType.OnInSert,IsIdentify =true)]
        public long Id { get; set; }
        [Column(ColumnName ="VIEWTYPE")]
        public ViewType Type { get; set; }

        [Column(ColumnName ="FILENAME")]
        public string FileName { get; set; }
        [Column(ColumnName ="FILECONTENT")]
        public string FileContent { get; set; }

    }
}