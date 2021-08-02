using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class PsychologicalExam
    {
        public string PsychologicalExamId { get; set; }
        public string Name { get; set; }

        public string NivelCtrlId { get; set; }
        public string CategoriaCtrlId { get; set; }
        public string InterpretacionCtrlId { get; set; }

        public bool HasLogic { get; set; }
    }
}
