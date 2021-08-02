using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class Matriz
    {
        public string ServiceId { get; set; }
        public string PersonId { get; set; }
        public DateTime? FechaNacimiento { get; set; }

        public string Proveedor { get; set; }
        public string Colaborador { get; set; }
        public string Dni { get; set; }
        public int? Edad { get; set; }
        public string Sexo { get; set; }
        public DateTime? FechaEvaluacion { get; set; }
        public string EmpresaEmpleadora { get; set; }
        public string Ruc { get; set; }
        public string TipoEvaluacion { get; set; }
        public string PerfiExamenOcupacional { get; set; }
        public string PuestoTrabajo { get; set; }
        public string Area { get; set; }
        public string Actividad { get; set; }

        public string AntecedentesPersonales { get; set; }
        public string Alergias { get; set; }
        public string PASistolica { get; set; }
        public string PADiastolica { get; set; }
        public string Peso { get; set; }
        public string Talla { get; set; }
        public string Imc { get; set; }
        public string DXImc { get; set; }
        public string Hb { get; set; }
        public string GrupoSanguineo { get; set; }
        public string FactorSanguineo { get; set; }
        public string Glicemia { get; set; }
        public string Colesterol { get; set; }
        public string Triglicerido { get; set; }
        public string ExOrina { get; set; }
        public string Toxicologico { get; set; }
        public string DxEKG { get; set; }
        public string AguVisualLejosSinCorreOD { get; set; }//no
        public string AguVisualLejosSinCorreOI { get; set; }//no
        public string AguVisualCercaSinCorreOD { get; set; }//no
        public string AguVisualCercaSinCorreOI { get; set; }//no
        public string AguVisualLejosConCorreOD { get; set; }//no
        public string AguVisualLejosConCorreOI { get; set; }//no
        public string AguVisualCercaConCorreOD { get; set; }//no
        public string AguVisualCercaConCorreOI { get; set; }//no
        public string DxAgudezaVisual { get; set; }//no
        public string DxVisionColores { get; set; }//no
        public string DxEsteropsis { get; set; }//no
        public string DxOftalmicoOtros { get; set; }//no
        public string DxOdontologico { get; set; }//no
        public string AudiometriaOI { get; set; }//no
        public string AudiometriaOD { get; set; }//no
        public string OtoscopiaOI { get; set; }//no
        public string OtoscopiaOD { get; set; }//no
        public string DxEspirometrico { get; set; }//no
        public string DxRadiografiaTorax { get; set; }//no
        public string DxNeumocosis { get; set; }//no
        public string DxPsicologia { get; set; }//no
        public string TestAlturaMayor18Mts { get; set; }//no
        public string Aptitud { get; set; }
        public string Recomendaciones { get; set; }
        public string Restricciones { get; set; }

        public string v_CustomerOrganizationId{ get; set; }
    }
}
