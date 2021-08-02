using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ReportCuestionarioEspirometria
    {
        public string IdServicio { get; set; }
        public string IdComponent { get; set; }

        public string Dni { get; set; }
        public string Puesto { get; set; }
        public DateTime? Fecha { get; set; }
        public string NombreTrabajador { get; set; }
        public DateTime? FechaNacimineto { get; set; }
        public int Edad { get; set; }
        public int Genero { get; set; }
        public string Pregunta1ASiNo { get; set; }
        public string Pregunta2ASiNo { get; set; }
        public string Pregunta3ASiNo { get; set; }
        public string Pregunta4ASiNo { get; set; }
        public string Pregunta5ASiNo { get; set; }


        public string HemoptisisSiNo { get; set; }
        public string PseumotoraxSiNo { get; set; }
        public string TraqueostomiaSiNo { get; set; }
        public string SondaPleuralSiNo { get; set; }
        public string AneurismaSiNo { get; set; }
        public string EmboliaSiNo { get; set; }
        public string InfartoSiNo { get; set; }
        public string InestabilidadSiNo { get; set; }
        public string FiebreSiNo { get; set; }
        public string EmbarazoAvanzadoSiNo { get; set; }
        public string EmbarazoComplicadoSiNo { get; set; }

        public string Pregunta1BSiNo { get; set; }
        public string Pregunta2BSiNo { get; set; }
        public string Pregunta3BSiNo { get; set; }
        public string Pregunta4BSiNo { get; set; }
        public string Pregunta5BSiNo { get; set; }
        public string Pregunta6BSiNo { get; set; }
        public string Pregunta7BSiNo { get; set; }

        public byte[] FirmaTrabajador { get; set; }
        public byte[] HuellaTrabajador { get; set; }

        public byte[] Logo { get; set; }

        public byte[] b_Logo { get; set; }
        public string EmpresaPropietaria { get; set; }
        public string EmpresaPropietariaDireccion { get; set; }
        public string EmpresaPropietariaTelefono { get; set; }
        public string EmpresaPropietariaEmail { get; set; }

        // NUEVO CUESTIONARIO

        public string CUESTIONARIO_EXCLUSION_TUVO_DESPRENDIMIENTO_RETINA { get; set; }
        public string CUESTIONARIO_EXCLUSION_TENIDO_ATAQUE_CARDIACO { get; set; }
        public string CUESTIONARIO_EXCLUSION_ESTADO_HOSPITALIZADO { get; set; }
        public string CUESTIONARIO_EXCLUSION_USANDO_MEDICAMENTOS { get; set; }
        public string CUESTIONARIO_EXCLUSION_CASO_SER_MUJER { get; set; }
        public string CUESTIONARIO_EXCLUSION_HEMOPTISIS { get; set; }
        public string CUESTIONARIO_EXCLUSION_NEUMOTORAX { get; set; }
        public string CUESTIONARIO_EXCLUSION_TRAQUEOSTOMIA { get; set; }
        public string CUESTIONARIO_EXCLUSION_SONDA_PLEURAL { get; set; }
        public string CUESTIONARIO_EXCLUSION_ANEURISMA_ABDOMEN { get; set; }
        public string CUESTIONARIO_EXCLUSION_EMBOLIA_PULMONAR { get; set; }
        public string CUESTIONARIO_EXCLUSION_INFARTO_RECIENTE { get; set; }
        public string CUESTIONARIO_EXCLUSION_INESTABILIDAD_CV { get; set; }
        public string CUESTIONARIO_EXCLUSION_FIEBRE_NAUSEA_VOMITO { get; set; }
        public string CUESTIONARIO_EXCLUSION_EMBARAZO_COMPLICADO { get; set; }

        public string CUESTIONARIO_DEBEN_HACER_TUVO_INFECCION_RESPIRATORIA { get; set; }
        public string CUESTIONARIO_DEBEN_HACER_TUVO_INFECCION_OIDO { get; set; }
        public string CUESTIONARIO_DEBEN_HACER_USA_AEROSOLES { get; set; }
        public string CUESTIONARIO_DEBEN_HACER_USADO_MEDICAMENTO_BRONCODILATADOR { get; set; }
        public string CUESTIONARIO_DEBEN_HACER_FUMO_TIPO_DE_CIGARRO { get; set; }
        public string CUESTIONARIO_DEBEN_HACER_CUANTOS { get; set; }
        public string CUESTIONARIO_DEBEN_HACER_REALIZO_EJERCICIO_FUERTE { get; set; }
        public string ESPIROMETRIA_CUESTIONARIO_EXCLUSION_EMBARAZO_AVANZADO { get; set; }

        public int i_TipEso { get; set; }
        public string v_TipEso { get; set; }

        public byte[] LogoEmpresaCliente { get; set; }
        public string RazonSocialEmpresaCliente { get; set; }
    }
}
