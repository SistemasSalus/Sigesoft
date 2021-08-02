//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:16:07
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Sigesoft.Server.WebClientAdmin.DAL;

namespace Sigesoft.Server.WebClientAdmin.BE
{

    /// <summary>
    /// Assembler for <see cref="headcount"/> and <see cref="headcountDto"/>.
    /// </summary>
    public static partial class headcountAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="headcountDto"/> converted from <see cref="headcount"/>.</param>
        static partial void OnDTO(this headcount entity, headcountDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="headcount"/> converted from <see cref="headcountDto"/>.</param>
        static partial void OnEntity(this headcountDto dto, headcount entity);

        /// <summary>
        /// Converts this instance of <see cref="headcountDto"/> to an instance of <see cref="headcount"/>.
        /// </summary>
        /// <param name="dto"><see cref="headcountDto"/> to convert.</param>
        public static headcount ToEntity(this headcountDto dto)
        {
            if (dto == null) return null;

            var entity = new headcount();

            entity.i_HcId = dto.i_HcId;
            entity.v_Nif = dto.v_Nif;
            entity.v_NombreCompleto = dto.v_NombreCompleto;
            entity.v_Soc = dto.v_Soc;
            entity.v_Sharp = dto.v_Sharp;
            entity.v_NroPers = dto.v_NroPers;
            entity.v_Apellido = dto.v_Apellido;
            entity.v_SegundoApellido = dto.v_SegundoApellido;
            entity.v_NombrePila = dto.v_NombrePila;
            entity.v_ClaveSexo = dto.v_ClaveSexo;
            entity.v_Aliq = dto.v_Aliq;
            entity.v_AreaNomina = dto.v_AreaNomina;
            entity.v_SdvPer = dto.v_SdvPer;
            entity.v_SubDivPersona = dto.v_SubDivPersona;
            entity.v_StatusOcupacion = dto.v_StatusOcupacion;
            entity.v_Cife = dto.v_Cife;
            entity.v_ClaseFecha = dto.v_ClaseFecha;
            entity.d_Fecha = dto.d_Fecha;
            entity.v_GrupoPersonal = dto.v_GrupoPersonal;
            entity.v_Posicion = dto.v_Posicion;
            entity.v_PosicionNombre = dto.v_PosicionNombre;
            entity.v_UnOrg = dto.v_UnOrg;
            entity.v_UnidadOrganizativa = dto.v_UnidadOrganizativa;
            entity.v_IndicadorExceptions = dto.v_IndicadorExceptions;
            entity.v_DivP = dto.v_DivP;
            entity.v_DivisionPersonal = dto.v_DivisionPersonal;
            entity.v_CECoste = dto.v_CECoste;
            entity.v_CentroCoste = dto.v_CentroCoste;
            entity.v_RelacionLaboral = dto.v_RelacionLaboral;
            entity.v_Apers = dto.v_Apers;
            entity.v_Area = dto.v_Area;
            entity.v_GrProf = dto.v_GrProf;
            entity.d_Desde = dto.d_Desde;
            entity.d_Hasta = dto.d_Hasta;
            entity.v_ClaseContrato = dto.v_ClaseContrato;
            entity.v_FinContrato = dto.v_FinContrato;
            entity.d_FechaNacimiento = dto.d_FechaNacimiento;
            entity.v_EPS = dto.v_EPS;
            entity.v_Sindicato = dto.v_Sindicato;
            entity.i_Edad = dto.i_Edad;
            entity.i_IsDeleted = dto.i_IsDeleted;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="headcount"/> to an instance of <see cref="headcountDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="headcount"/> to convert.</param>
        public static headcountDto ToDTO(this headcount entity)
        {
            if (entity == null) return null;

            var dto = new headcountDto();

            dto.i_HcId = entity.i_HcId;
            dto.v_Nif = entity.v_Nif;
            dto.v_NombreCompleto = entity.v_NombreCompleto;
            dto.v_Soc = entity.v_Soc;
            dto.v_Sharp = entity.v_Sharp;
            dto.v_NroPers = entity.v_NroPers;
            dto.v_Apellido = entity.v_Apellido;
            dto.v_SegundoApellido = entity.v_SegundoApellido;
            dto.v_NombrePila = entity.v_NombrePila;
            dto.v_ClaveSexo = entity.v_ClaveSexo;
            dto.v_Aliq = entity.v_Aliq;
            dto.v_AreaNomina = entity.v_AreaNomina;
            dto.v_SdvPer = entity.v_SdvPer;
            dto.v_SubDivPersona = entity.v_SubDivPersona;
            dto.v_StatusOcupacion = entity.v_StatusOcupacion;
            dto.v_Cife = entity.v_Cife;
            dto.v_ClaseFecha = entity.v_ClaseFecha;
            dto.d_Fecha = entity.d_Fecha;
            dto.v_GrupoPersonal = entity.v_GrupoPersonal;
            dto.v_Posicion = entity.v_Posicion;
            dto.v_PosicionNombre = entity.v_PosicionNombre;
            dto.v_UnOrg = entity.v_UnOrg;
            dto.v_UnidadOrganizativa = entity.v_UnidadOrganizativa;
            dto.v_IndicadorExceptions = entity.v_IndicadorExceptions;
            dto.v_DivP = entity.v_DivP;
            dto.v_DivisionPersonal = entity.v_DivisionPersonal;
            dto.v_CECoste = entity.v_CECoste;
            dto.v_CentroCoste = entity.v_CentroCoste;
            dto.v_RelacionLaboral = entity.v_RelacionLaboral;
            dto.v_Apers = entity.v_Apers;
            dto.v_Area = entity.v_Area;
            dto.v_GrProf = entity.v_GrProf;
            dto.d_Desde = entity.d_Desde;
            dto.d_Hasta = entity.d_Hasta;
            dto.v_ClaseContrato = entity.v_ClaseContrato;
            dto.v_FinContrato = entity.v_FinContrato;
            dto.d_FechaNacimiento = entity.d_FechaNacimiento;
            dto.v_EPS = entity.v_EPS;
            dto.v_Sindicato = entity.v_Sindicato;
            dto.i_Edad = entity.i_Edad;
            dto.i_IsDeleted = entity.i_IsDeleted;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="headcountDto"/> to an instance of <see cref="headcount"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<headcount> ToEntities(this IEnumerable<headcountDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="headcount"/> to an instance of <see cref="headcountDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<headcountDto> ToDTOs(this IEnumerable<headcount> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
