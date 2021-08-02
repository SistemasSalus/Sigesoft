using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class PsychologicalExamDetail
    {
        public string PsychologicalExamId { get; set; }
        public int PsychologicalInterpretationId { get; set; }
        public int? AnalyzingValue1 { get; set; }
        public int? AnalyzingValue2 { get; set; }
        public int Level { get; set; }
        public string Category { get; set; }

        public string PsychologicalInterpretationName { get; set; }
        public string PsychologicalExamName { get; set; }


        public string NivelCtrlId { get; set; }
        public string CategoriaCtrlId { get; set; }
        public string InterpretacionCtrlId { get; set; }

        public bool HasLogic { get; set; }
    }
}
