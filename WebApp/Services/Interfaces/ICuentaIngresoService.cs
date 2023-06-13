using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BanCobradotas.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Services.Interfaces;

public interface ICuentaIngresoService
{
    Task<(CuentaIngreso? cuenta, string? err)> GetCuentaIngreso(string usuario, string contrasena);

    Task<(Usuario? usr, string? err)> RequestCuentaUsuario(Usuario usr);
}