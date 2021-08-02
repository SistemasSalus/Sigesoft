//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:29
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
    /// Assembler for <see cref="calendar"/> and <see cref="calendarDto"/>.
    /// </summary>
    public static partial class calendarAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="calendarDto"/> converted from <see cref="calendar"/>.</param>
        static partial void OnDTO(this calendar entity, calendarDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="calendar"/> converted from <see cref="calendarDto"/>.</param>
        static partial void OnEntity(this calendarDto dto, calendar entity);

        /// <summary>
        /// Converts this instance of <see cref="calendarDto"/> to an instance of <see cref="calendar"/>.
        /// </summary>
        /// <param name="dto"><see cref="calendarDto"/> to convert.</param>
        public static calendar ToEntity(this calendarDto dto)
        {
            if (dto == null) return null;

            var entity = new calendar();

            entity.v_CalendarId = dto.v_CalendarId;
            entity.v_PersonId = dto.v_PersonId;
            entity.v_ServiceId = dto.v_ServiceId;
            entity.d_DateTimeCalendar = dto.d_DateTimeCalendar;
            entity.d_CircuitStartDate = dto.d_CircuitStartDate;
            entity.d_EntryTimeCM = dto.d_EntryTimeCM;
            entity.i_ServiceTypeId = dto.i_ServiceTypeId;
            entity.i_CalendarStatusId = dto.i_CalendarStatusId;
            entity.i_ServiceId = dto.i_ServiceId;
            entity.v_ProtocolId = dto.v_ProtocolId;
            entity.i_NewContinuationId = dto.i_NewContinuationId;
            entity.i_LineStatusId = dto.i_LineStatusId;
            entity.i_IsVipId = dto.i_IsVipId;
            entity.i_IsDeleted = dto.i_IsDeleted;
            entity.i_InsertUserId = dto.i_InsertUserId;
            entity.d_InsertDate = dto.d_InsertDate;
            entity.i_UpdateUserId = dto.i_UpdateUserId;
            entity.d_UpdateDate = dto.d_UpdateDate;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="calendar"/> to an instance of <see cref="calendarDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="calendar"/> to convert.</param>
        public static calendarDto ToDTO(this calendar entity)
        {
            if (entity == null) return null;

            var dto = new calendarDto();

            dto.v_CalendarId = entity.v_CalendarId;
            dto.v_PersonId = entity.v_PersonId;
            dto.v_ServiceId = entity.v_ServiceId;
            dto.d_DateTimeCalendar = entity.d_DateTimeCalendar;
            dto.d_CircuitStartDate = entity.d_CircuitStartDate;
            dto.d_EntryTimeCM = entity.d_EntryTimeCM;
            dto.i_ServiceTypeId = entity.i_ServiceTypeId;
            dto.i_CalendarStatusId = entity.i_CalendarStatusId;
            dto.i_ServiceId = entity.i_ServiceId;
            dto.v_ProtocolId = entity.v_ProtocolId;
            dto.i_NewContinuationId = entity.i_NewContinuationId;
            dto.i_LineStatusId = entity.i_LineStatusId;
            dto.i_IsVipId = entity.i_IsVipId;
            dto.i_IsDeleted = entity.i_IsDeleted;
            dto.i_InsertUserId = entity.i_InsertUserId;
            dto.d_InsertDate = entity.d_InsertDate;
            dto.i_UpdateUserId = entity.i_UpdateUserId;
            dto.d_UpdateDate = entity.d_UpdateDate;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="calendarDto"/> to an instance of <see cref="calendar"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<calendar> ToEntities(this IEnumerable<calendarDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="calendar"/> to an instance of <see cref="calendarDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<calendarDto> ToDTOs(this IEnumerable<calendar> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
