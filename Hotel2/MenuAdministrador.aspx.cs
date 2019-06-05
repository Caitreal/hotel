﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hotel2
{
    public partial class MenuAdministrador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            comprobarSesion();
        }

        protected void btnGananciaMes_Click(object sender, EventArgs e)
        {
            Response.Redirect("verGananciasMesAdministrador.aspx");
        }

        protected void btnGananciaRango_Click(object sender, EventArgs e)
        {
            Response.Redirect("verGananciaSinLimiteAdministrador.aspx");
        }

        protected void btnLista_Click(object sender, EventArgs e)
        {
            Response.Redirect("ListadoHabitaciones.aspx");
        }

        protected void btnCambiar_Click(object sender, EventArgs e)
        {
            Response.Redirect("cambiarDescuento.aspx");
        }

        private void comprobarSesion()
        {
            Usuario user = Session["conectado"] as Usuario;
            if (user != null)
            {
                if (!user.TipoUsuario.Nombre.Equals("ADMINISTRADOR"))
                {
                    Response.Redirect("Default.aspx");
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
    }
}