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
    /// Assembler for <see cref="uspgetemployercompaniesResult"/> and <see cref="uspgetemployercompaniesResultDto"/>.
    /// </summary>
    public static partial class uspgetemployercompaniesResultAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="uspgetemployercompaniesResultDto"/> converted from <see cref="uspgetemployercompaniesResult"/>.</param>
        static partial void OnDTO(this uspgetemployercompaniesResult entity, uspgetemployercompaniesResultDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="uspgetemployercompaniesResult"/> converted from <see cref="uspgetemployercompaniesResultDto"/>.</param>
        static partial void OnEntity(this uspgetemployercompaniesResultDto dto, uspgetemployercompaniesResult entity);

        /// <summary>
        /// Converts this instance of <see cref="uspgetemployercompaniesResultDto"/> to an instance of <see cref="uspgetemployercompaniesResult"/>.
        /// </summary>
        /// <param name="dto"><see cref="uspgetemployercompaniesResultDto"/> to convert.</param>
        public static uspgetemployercompaniesResult ToEntity(this uspgetemployercompaniesResultDto dto)
        {
            if (dto == null) return null;

            var entity = new uspgetemployercompaniesResult();

            entity.EmployerCompanyId = dto.EmployerCompanyId;
            entity.NationalIDNumber = dto.NationalIDNumber;
            entity.Name = dto.Name;
            entity.rowguid = dto.rowguid;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="uspgetemployercompaniesResult"/> to an instance of <see cref="uspgetemployercompaniesResultDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="uspgetemployercompaniesResult"/> to convert.</param>
        public static uspgetemployercompaniesResultDto ToDTO(this uspgetemployercompaniesResult entity)
        {
            if (entity == null) return null;

            var dto = new uspgetemployercompaniesResultDto();

            dto.EmployerCompanyId = entity.EmployerCompanyId;
            dto.NationalIDNumber = entity.NationalIDNumber;
            dto.Name = entity.Name;
            dto.rowguid = entity.rowguid;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="uspgetemployercompaniesResultDto"/> to an instance of <see cref="uspgetemployercompaniesResult"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<uspgetemployercompaniesResult> ToEntities(this IEnumerable<uspgetemployercompaniesResultDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="uspgetemployercompaniesResult"/> to an instance of <see cref="uspgetemployercompaniesResultDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<uspgetemployercompaniesResultDto> ToDTOs(this IEnumerable<uspgetemployercompaniesResult> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
