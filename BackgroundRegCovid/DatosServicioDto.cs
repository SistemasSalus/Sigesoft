using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackgroundRegCovid
{
    public class DatosServicioDto
    {
        public string ServiceId { get; set; }
        public string OrganizationId { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public string Documento { get; set; }
        public string ComponentId { get; set; }
        public DateTime FechaServicio { get; set; }
        public DateTime FechaInsert { get; set; }
        public int? TipoFormato { get; set; }
        public int? TipoEmpresa { get; set; }
        public string Sede { get; set; }
        public int? SedeId { get; set; }
        public string Tecnico { get; set; }
        public string NombresCompleto { get; set; }
        public string EmpresaEmpleadora { get; set; }
    }
}
