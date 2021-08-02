//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2020/07/24 - 14:16:02
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
    /// Assembler for <see cref="uspgetresultResult"/> and <see cref="uspgetresultResultDto"/>.
    /// </summary>
    public static partial class uspgetresultResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="uspgetresultResultDto"/> converted from <see cref="uspgetresultResult"/>.</param>
        static partial void OnDTO(this uspgetresultResult entity, uspgetresultResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="uspgetresultResult"/> converted from <see cref="uspgetresultResultDto"/>.</param>
        static partial void OnEntity(this uspgetresultResultDto dto, uspgetresultResult entity);

        /// <summary>
        /// Converts this instance of <see cref="uspgetresultResultDto"/> to an instance of <see cref="uspgetresultResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="uspgetresultResultDto"/> to convert.</param>
        public static uspgetresultResult ToEntity(this uspgetresultResultDto dto)
        {
            if (dto == null) return null;

            var entity = new uspgetresultResult();

            entity.v_ServiceId = dto.v_ServiceId;
            entity.v_FirstLastName = dto.v_FirstLastName;
            entity.v_SecondLastName = dto.v_SecondLastName;
            entity.v_FirstName = dto.v_FirstName;
            entity.v_Name = dto.v_Name;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="uspgetresultResult"/> to an instance of <see cref="uspgetresultResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="uspgetresultResult"/> to convert.</param>
        public static uspgetresultResultDto ToDTO(this uspgetresultResult entity)
        {
            if (entity == null) return null;

            var dto = new uspgetresultResultDto();

            dto.v_ServiceId = entity.v_ServiceId;
            dto.v_FirstLastName = entity.v_FirstLastName;
            dto.v_SecondLastName = entity.v_SecondLastName;
            dto.v_FirstName = entity.v_FirstName;
            dto.v_Name = entity.v_Name;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="uspgetresultResultDto"/> to an instance of <see cref="uspgetresultResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<uspgetresultResult> ToEntities(this IEnumerable<uspgetresultResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="uspgetresultResult"/> to an instance of <see cref="uspgetresultResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<uspgetresultResultDto> ToDTOs(this IEnumerable<uspgetresultResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
