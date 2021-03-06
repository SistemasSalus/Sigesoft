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
    /// Assembler for <see cref="regcovid_get_systemparametersResult"/> and <see cref="regcovid_get_systemparametersResultDto"/>.
    /// </summary>
    public static partial class regcovid_get_systemparametersResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_get_systemparametersResultDto"/> converted from <see cref="regcovid_get_systemparametersResult"/>.</param>
        static partial void OnDTO(this regcovid_get_systemparametersResult entity, regcovid_get_systemparametersResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_get_systemparametersResult"/> converted from <see cref="regcovid_get_systemparametersResultDto"/>.</param>
        static partial void OnEntity(this regcovid_get_systemparametersResultDto dto, regcovid_get_systemparametersResult entity);

        /// <summary>
        /// Converts this instance of <see cref="regcovid_get_systemparametersResultDto"/> to an instance of <see cref="regcovid_get_systemparametersResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="regcovid_get_systemparametersResultDto"/> to convert.</param>
        public static regcovid_get_systemparametersResult ToEntity(this regcovid_get_systemparametersResultDto dto)
        {
            if (dto == null) return null;

            var entity = new regcovid_get_systemparametersResult();

            entity.ParameterId = dto.ParameterId;
            entity.Value1 = dto.Value1;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="regcovid_get_systemparametersResult"/> to an instance of <see cref="regcovid_get_systemparametersResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="regcovid_get_systemparametersResult"/> to convert.</param>
        public static regcovid_get_systemparametersResultDto ToDTO(this regcovid_get_systemparametersResult entity)
        {
            if (entity == null) return null;

            var dto = new regcovid_get_systemparametersResultDto();

            dto.ParameterId = entity.ParameterId;
            dto.Value1 = entity.Value1;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_get_systemparametersResultDto"/> to an instance of <see cref="regcovid_get_systemparametersResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<regcovid_get_systemparametersResult> ToEntities(this IEnumerable<regcovid_get_systemparametersResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="regcovid_get_systemparametersResult"/> to an instance of <see cref="regcovid_get_systemparametersResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<regcovid_get_systemparametersResultDto> ToDTOs(this IEnumerable<regcovid_get_systemparametersResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
