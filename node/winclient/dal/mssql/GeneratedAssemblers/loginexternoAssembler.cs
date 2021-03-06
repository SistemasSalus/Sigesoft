//-------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by EntitiesToDTOs.v3.2 (entitiestodtos.codeplex.com).
//     Timestamp: 2021/05/17 - 17:34:35
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
    /// Assembler for <see cref="loginexterno"/> and <see cref="loginexternoDto"/>.
    /// </summary>
    public static partial class loginexternoAssembler
    {
        /// <summary>
        /// Invoked when <see cref="ToDTO"/> operation is about to return.
        /// </summary>
        /// <param name="dto"><see cref="loginexternoDto"/> converted from <see cref="loginexterno"/>.</param>
        static partial void OnDTO(this loginexterno entity, loginexternoDto dto);

        /// <summary>
        /// Invoked when <see cref="ToEntity"/> operation is about to return.
        /// </summary>
        /// <param name="entity"><see cref="loginexterno"/> converted from <see cref="loginexternoDto"/>.</param>
        static partial void OnEntity(this loginexternoDto dto, loginexterno entity);

        /// <summary>
        /// Converts this instance of <see cref="loginexternoDto"/> to an instance of <see cref="loginexterno"/>.
        /// </summary>
        /// <param name="dto"><see cref="loginexternoDto"/> to convert.</param>
        public static loginexterno ToEntity(this loginexternoDto dto)
        {
            if (dto == null) return null;

            var entity = new loginexterno();

            entity.i_Id = dto.i_Id;
            entity.v_Usuario = dto.v_Usuario;
            entity.v_Password = dto.v_Password;
            entity.v_Nombres = dto.v_Nombres;
            entity.v_Apellidos = dto.v_Apellidos;
            entity.v_OrganizationId = dto.v_OrganizationId;
            entity.d_FechaRegistro = dto.d_FechaRegistro;

            dto.OnEntity(entity);

            return entity;
        }

        /// <summary>
        /// Converts this instance of <see cref="loginexterno"/> to an instance of <see cref="loginexternoDto"/>.
        /// </summary>
        /// <param name="entity"><see cref="loginexterno"/> to convert.</param>
        public static loginexternoDto ToDTO(this loginexterno entity)
        {
            if (entity == null) return null;

            var dto = new loginexternoDto();

            dto.i_Id = entity.i_Id;
            dto.v_Usuario = entity.v_Usuario;
            dto.v_Password = entity.v_Password;
            dto.v_Nombres = entity.v_Nombres;
            dto.v_Apellidos = entity.v_Apellidos;
            dto.v_OrganizationId = entity.v_OrganizationId;
            dto.d_FechaRegistro = entity.d_FechaRegistro;

            entity.OnDTO(dto);

            return dto;
        }

        /// <summary>
        /// Converts each instance of <see cref="loginexternoDto"/> to an instance of <see cref="loginexterno"/>.
        /// </summary>
        /// <param name="dtos"></param>
        /// <returns></returns>
        public static List<loginexterno> ToEntities(this IEnumerable<loginexternoDto> dtos)
        {
            if (dtos == null) return null;

            return dtos.Select(e => e.ToEntity()).ToList();
        }

        /// <summary>
        /// Converts each instance of <see cref="loginexterno"/> to an instance of <see cref="loginexternoDto"/>.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<loginexternoDto> ToDTOs(this IEnumerable<loginexterno> entities)
        {
            if (entities == null) return null;

            return entities.Select(e => e.ToDTO()).ToList();
        }

    }
}
