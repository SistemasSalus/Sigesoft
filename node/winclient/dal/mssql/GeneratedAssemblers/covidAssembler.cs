//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:31
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//-------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Linq;
using Sigesoft.Node.WinClient.DAL;

namespace Sigesoft.Node.WinClient.BE
{

    /// <summary>
    /// Assembler for <see cref="covid"/> and <see cref="covidDto"/>.
    /// </summary>
    public static partial class covidAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="covidDto"/> converted from <see cref="covid"/>.</param>
        static partial void OnDTO(this covid entity, covidDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="covid"/> converted from <see cref="covidDto"/>.</param>
        static partial void OnEntity(this covidDto dto, covid entity);

        /// <summary>
        /// Converts this instance of <see cref="covidDto"/> to an instance of <see cref="covid"/>.
        /// </summary>
        /// <param name="dto"><see cref="covidDto"/> to convert.</param>
        public static covid ToEntity(this covidDto dto)
        {
            if (dto == null) return null;

            var entity = new covid();

            entity.v_CovidId = dto.v_CovidId;
            entity.v_ServiceId = dto.v_ServiceId;
            entity.i_Triaje = dto.i_Triaje;
            entity.i_Encuesta = dto.i_Encuesta;
            entity.i_Laboratorio = dto.i_Laboratorio;
            entity.i_Certificado = dto.i_Certificado;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="covid"/> to an instance of <see cref="covidDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="covid"/> to convert.</param>
        public static covidDto ToDTO(this covid entity)
        {
            if (entity == null) return null;

            var dto = new covidDto();

            dto.v_CovidId = entity.v_CovidId;
            dto.v_ServiceId = entity.v_ServiceId;
            dto.i_Triaje = entity.i_Triaje;
            dto.i_Encuesta = entity.i_Encuesta;
            dto.i_Laboratorio = entity.i_Laboratorio;
            dto.i_Certificado = entity.i_Certificado;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="covidDto"/> to an instance of <see cref="covid"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<covid> ToEntities(this IEnumerable<covidDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="covid"/> to an instance of <see cref="covidDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<covidDto> ToDTOs(this IEnumerable<covid> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
