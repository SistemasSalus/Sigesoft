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
    /// Assembler for <see cref="regcovid_scheduleworkerResult"/> and <see cref="regcovid_scheduleworkerResultDto"/>.
    /// </summary>
    public static partial class regcovid_scheduleworkerResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_scheduleworkerResultDto"/> converted from <see cref="regcovid_scheduleworkerResult"/>.</param>
        static partial void OnDTO(this regcovid_scheduleworkerResult entity, regcovid_scheduleworkerResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_scheduleworkerResult"/> converted from <see cref="regcovid_scheduleworkerResultDto"/>.</param>
        static partial void OnEntity(this regcovid_scheduleworkerResultDto dto, regcovid_scheduleworkerResult entity);

        /// <summary>
        /// Converts this instance of <see cref="regcovid_scheduleworkerResultDto"/> to an instance of <see cref="regcovid_scheduleworkerResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_scheduleworkerResultDto"/> to convert.</param>
        public static regcovid_scheduleworkerResult ToEntity(this regcovid_scheduleworkerResultDto dto)
        {
            if (dto == null) return null;

            var entity = new regcovid_scheduleworkerResult();

            entity.column0 = dto.column0;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="regcovid_scheduleworkerResult"/> to an instance of <see cref="regcovid_scheduleworkerResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_scheduleworkerResult"/> to convert.</param>
        public static regcovid_scheduleworkerResultDto ToDTO(this regcovid_scheduleworkerResult entity)
        {
            if (entity == null) return null;

            var dto = new regcovid_scheduleworkerResultDto();

            dto.column0 = entity.column0;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_scheduleworkerResultDto"/> to an instance of <see cref="regcovid_scheduleworkerResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<regcovid_scheduleworkerResult> ToEntities(this IEnumerable<regcovid_scheduleworkerResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_scheduleworkerResult"/> to an instance of <see cref="regcovid_scheduleworkerResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<regcovid_scheduleworkerResultDto> ToDTOs(this IEnumerable<regcovid_scheduleworkerResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}