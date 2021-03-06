//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:25
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
    /// Assembler for <see cref="regcovid_select_brandResult"/> and <see cref="regcovid_select_brandResultDto"/>.
    /// </summary>
    public static partial class regcovid_select_brandResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_select_brandResultDto"/> converted from <see cref="regcovid_select_brandResult"/>.</param>
        static partial void OnDTO(this regcovid_select_brandResult entity, regcovid_select_brandResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_select_brandResult"/> converted from <see cref="regcovid_select_brandResultDto"/>.</param>
        static partial void OnEntity(this regcovid_select_brandResultDto dto, regcovid_select_brandResult entity);

        /// <summary>
        /// Converts this instance of <see cref="regcovid_select_brandResultDto"/> to an instance of <see cref="regcovid_select_brandResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_select_brandResultDto"/> to convert.</param>
        public static regcovid_select_brandResult ToEntity(this regcovid_select_brandResultDto dto)
        {
            if (dto == null) return null;

            var entity = new regcovid_select_brandResult();

            entity.iBrandId = dto.iBrandId;
            entity.vBrandName = dto.vBrandName;
            entity.iBrandState = dto.iBrandState;
            entity.iDeleted = dto.iDeleted;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="regcovid_select_brandResult"/> to an instance of <see cref="regcovid_select_brandResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_select_brandResult"/> to convert.</param>
        public static regcovid_select_brandResultDto ToDTO(this regcovid_select_brandResult entity)
        {
            if (entity == null) return null;

            var dto = new regcovid_select_brandResultDto();

            dto.iBrandId = entity.iBrandId;
            dto.vBrandName = entity.vBrandName;
            dto.iBrandState = entity.iBrandState;
            dto.iDeleted = entity.iDeleted;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_select_brandResultDto"/> to an instance of <see cref="regcovid_select_brandResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<regcovid_select_brandResult> ToEntities(this IEnumerable<regcovid_select_brandResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_select_brandResult"/> to an instance of <see cref="regcovid_select_brandResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<regcovid_select_brandResultDto> ToDTOs(this IEnumerable<regcovid_select_brandResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
