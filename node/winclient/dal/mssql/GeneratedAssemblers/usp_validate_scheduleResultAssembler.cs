//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:28
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
    /// Assembler for <see cref="usp_validate_scheduleResult"/> and <see cref="usp_validate_scheduleResultDto"/>.
    /// </summary>
    public static partial class usp_validate_scheduleResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="usp_validate_scheduleResultDto"/> converted from <see cref="usp_validate_scheduleResult"/>.</param>
        static partial void OnDTO(this usp_validate_scheduleResult entity, usp_validate_scheduleResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="usp_validate_scheduleResult"/> converted from <see cref="usp_validate_scheduleResultDto"/>.</param>
        static partial void OnEntity(this usp_validate_scheduleResultDto dto, usp_validate_scheduleResult entity);

        /// <summary>
        /// Converts this instance of <see cref="usp_validate_scheduleResultDto"/> to an instance of <see cref="usp_validate_scheduleResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="usp_validate_scheduleResultDto"/> to convert.</param>
        public static usp_validate_scheduleResult ToEntity(this usp_validate_scheduleResultDto dto)
        {
            if (dto == null) return null;

            var entity = new usp_validate_scheduleResult();

            entity.v_CalendarId = dto.v_CalendarId;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="usp_validate_scheduleResult"/> to an instance of <see cref="usp_validate_scheduleResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="usp_validate_scheduleResult"/> to convert.</param>
        public static usp_validate_scheduleResultDto ToDTO(this usp_validate_scheduleResult entity)
        {
            if (entity == null) return null;

            var dto = new usp_validate_scheduleResultDto();

            dto.v_CalendarId = entity.v_CalendarId;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_validate_scheduleResultDto"/> to an instance of <see cref="usp_validate_scheduleResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<usp_validate_scheduleResult> ToEntities(this IEnumerable<usp_validate_scheduleResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="usp_validate_scheduleResult"/> to an instance of <see cref="usp_validate_scheduleResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<usp_validate_scheduleResultDto> ToDTOs(this IEnumerable<usp_validate_scheduleResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
