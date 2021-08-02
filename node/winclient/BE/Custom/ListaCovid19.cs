using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sigesoft.Node.WinClient.BE
{
    public class ListaCovid19
    {
        public string Nodo { get; set; }
        public string Nombres { get; set; }
        public string Estado { get; set; }
        public string Apellidos { get; set; }
        public string Protocolo { get; set; }
        public string Empresa { get; set; }
        public string Puesto { get; set; }
        public string ServiceId { get; set; }
        public string PersonId { get; set; }
        public string ComponentId { get; set; }
        public string ServiceComponentId { get; set; }
        public int? Index { get; set; }
        public string EstadoTriaje { get; set; }
        public string EstadoLaboratorio { get; set; }
        public string OrganizationId { get; set; }
        public DateTime FechaServicio { get; set; }
        public string Aptitud { get; set; }
        public string Telefono { get; set; }

        public int CountNegativo  { get; set; }
        public int CountNoValido { get; set; }
        public int CountIgMPositivo { get; set; }
        public int CountIgGPositivo { get; set; }
        public int CountIgMIgGPositivo { get; set; }
    }
}
