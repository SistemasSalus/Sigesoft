//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:22
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
    /// Assembler for <see cref="getallcampaniascovidResult"/> and <see cref="getallcampaniascovidResultDto"/>.
    /// </summary>
    public static partial class getallcampaniascovidResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="getallcampaniascovidResultDto"/> converted from <see cref="getallcampaniascovidResult"/>.</param>
        static partial void OnDTO(this getallcampaniascovidResult entity, getallcampaniascovidResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="getallcampaniascovidResult"/> converted from <see cref="getallcampaniascovidResultDto"/>.</param>
        static partial void OnEntity(this getallcampaniascovidResultDto dto, getallcampaniascovidResult entity);

        /// <summary>
        /// Converts this instance of <see cref="getallcampaniascovidResultDto"/> to an instance of <see cref="getallcampaniascovidResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="getallcampaniascovidResultDto"/> to convert.</param>
        public static getallcampaniascovidResult ToEntity(this getallcampaniascovidResultDto dto)
        {
            if (dto == null) return null;

            var entity = new getallcampaniascovidResult();

            entity.id = dto.id;
            entity.Sede = dto.Sede;
            entity.Area = dto.Area;
            entity.DNI = dto.DNI;
            entity.ApellidoMaterno = dto.ApellidoMaterno;
            entity.ApellidoPaterno = dto.ApellidoPaterno;
            entity.Nombres = dto.Nombres;
            entity.Fecha = dto.Fecha;
            entity.Resultado = dto.Resultado;
            entity.Campa?a = dto.Campa?a;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="getallcampaniascovidResult"/> to an instance of <see cref="getallcampaniascovidResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="getallcampaniascovidResult"/> to convert.</param>
        public static getallcampaniascovidResultDto ToDTO(this getallcampaniascovidResult entity)
        {
            if (entity == null) return null;

            var dto = new getallcampaniascovidResultDto();

            dto.id = entity.id;
            dto.Sede = entity.Sede;
            dto.Area = entity.Area;
            dto.DNI = entity.DNI;
            dto.ApellidoMaterno = entity.ApellidoMaterno;
            dto.ApellidoPaterno = entity.ApellidoPaterno;
            dto.Nombres = entity.Nombres;
            dto.Fecha = entity.Fecha;
            dto.Resultado = entity.Resultado;
            dto.Campa?a = entity.Campa?a;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="getallcampaniascovidResultDto"/> to an instance of <see cref="getallcampaniascovidResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<getallcampaniascovidResult> ToEntities(this IEnumerable<getallcampaniascovidResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="getallcampaniascovidResult"/> to an instance of <see cref="getallcampaniascovidResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<getallcampaniascovidResultDto> ToDTOs(this IEnumerable<getallcampaniascovidResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
