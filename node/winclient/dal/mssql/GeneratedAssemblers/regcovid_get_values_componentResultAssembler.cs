//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:24
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
    /// Assembler for <see cref="regcovid_get_values_componentResult"/> and <see cref="regcovid_get_values_componentResultDto"/>.
    /// </summary>
    public static partial class regcovid_get_values_componentResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_get_values_componentResultDto"/> converted from <see cref="regcovid_get_values_componentResult"/>.</param>
        static partial void OnDTO(this regcovid_get_values_componentResult entity, regcovid_get_values_componentResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_get_values_componentResult"/> converted from <see cref="regcovid_get_values_componentResultDto"/>.</param>
        static partial void OnEntity(this regcovid_get_values_componentResultDto dto, regcovid_get_values_componentResult entity);

        /// <summary>
        /// Converts this instance of <see cref="regcovid_get_values_componentResultDto"/> to an instance of <see cref="regcovid_get_values_componentResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_get_values_componentResultDto"/> to convert.</param>
        public static regcovid_get_values_componentResult ToEntity(this regcovid_get_values_componentResultDto dto)
        {
            if (dto == null) return null;

            var entity = new regcovid_get_values_componentResult();

            entity.ComponentFieldId = dto.ComponentFieldId;
            entity.ServiceComponentFieldsId = dto.ServiceComponentFieldsId;
            entity.Value1 = dto.Value1;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="regcovid_get_values_componentResult"/> to an instance of <see cref="regcovid_get_values_componentResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_get_values_componentResult"/> to convert.</param>
        public static regcovid_get_values_componentResultDto ToDTO(this regcovid_get_values_componentResult entity)
        {
            if (entity == null) return null;

            var dto = new regcovid_get_values_componentResultDto();

            dto.ComponentFieldId = entity.ComponentFieldId;
            dto.ServiceComponentFieldsId = entity.ServiceComponentFieldsId;
            dto.Value1 = entity.Value1;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_get_values_componentResultDto"/> to an instance of <see cref="regcovid_get_values_componentResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<regcovid_get_values_componentResult> ToEntities(this IEnumerable<regcovid_get_values_componentResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_get_values_componentResult"/> to an instance of <see cref="regcovid_get_values_componentResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<regcovid_get_values_componentResultDto> ToDTOs(this IEnumerable<regcovid_get_values_componentResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
