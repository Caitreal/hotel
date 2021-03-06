﻿using Hotel2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hotel
{
    public partial class EliminarHabitacion : System.Web.UI.Page
    {
        public int Id { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var ok = false;
            var conectado = Session["conectado"] as Usuario;
            if (conectado != null)
            {
                if (conectado.TipoUsuario.Nombre == "ADMINISTRADOR")
                {
                    ok = true;
                }
            }
            if (!ok)
            {
                Response.Redirect("~/Default");
            }
            else
            {
                if (!IsPostBack)
                {
                    Id = Convert.ToInt32(Request.QueryString["id"]);
                    var db = new DB();
                    var habitacion = db.Habitacion.Find(Id);
                    db.Habitacion.Remove(habitacion);

                    db.SaveChanges();

                    Response.Redirect("ListadoHabitaciones");
                }
            }
            
        }
    }
}