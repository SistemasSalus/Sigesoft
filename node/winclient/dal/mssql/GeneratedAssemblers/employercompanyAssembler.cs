//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:33
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
    /// Assembler for <see cref="employercompany"/> and <see cref="employercompanyDto"/>.
    /// </summary>
    public static partial class employercompanyAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="employercompanyDto"/> converted from <see cref="employercompany"/>.</param>
        static partial void OnDTO(this employercompany entity, employercompanyDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="employercompany"/> converted from <see cref="employercompanyDto"/>.</param>
        static partial void OnEntity(this employercompanyDto dto, employercompany entity);

        /// <summary>
        /// Converts this instance of <see cref="employercompanyDto"/> to an instance of <see cref="employercompany"/>.
        /// </summary>
        /// <param name="dto"><see cref="employercompanyDto"/> to convert.</param>
        public static employercompany ToEntity(this employercompanyDto dto)
        {
            if (dto == null) return null;

            var entity = new employercompany();

            entity.EmployerCompanyId = dto.EmployerCompanyId;
            entity.NationalIDNumber = dto.NationalIDNumber;
            entity.Name = dto.Name;
            entity.CurrentFlag = dto.CurrentFlag;
            entity.rowguid = dto.rowguid;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="employercompany"/> to an instance of <see cref="employercompanyDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="employercompany"/> to convert.</param>
        public static employercompanyDto ToDTO(this employercompany entity)
        {
            if (entity == null) return null;

            var dto = new employercompanyDto();

            dto.EmployerCompanyId = entity.EmployerCompanyId;
            dto.NationalIDNumber = entity.NationalIDNumber;
            dto.Name = entity.Name;
            dto.CurrentFlag = entity.CurrentFlag;
            dto.rowguid = entity.rowguid;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="employercompanyDto"/> to an instance of <see cref="employercompany"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<employercompany> ToEntities(this IEnumerable<employercompanyDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="employercompany"/> to an instance of <see cref="employercompanyDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<employercompanyDto> ToDTOs(this IEnumerable<employercompany> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
