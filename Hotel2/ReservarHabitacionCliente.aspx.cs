﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hotel2
{
    public partial class Reservar_ : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            comprobarSesion();
        }
        private void LlenarDropListHabitacion(List<Habitacion> habitacionesFiltradas)
        {
            DropListHabitacion.DataSource = habitacionesFiltradas;
            DropListHabitacion.DataTextField = "Id";
            DropListHabitacion.DataValueField = "Id";
            DropListHabitacion.DataBind();
        }
       

        protected void btnReservar_Click(object sender, EventArgs e)
        {
            var db = new DB();
            var hoy = DateTime.Now;
            int idHabitacion = Convert.ToInt32(DropListHabitacion.SelectedValue);
            Cliente cliente = Session["conectado"] as Cliente;
            int descuentoCliente = Convert.ToInt32(cliente.TipoCliente.Descuento);
            Reserva reserva = new Reserva();
            reserva.Cliente = cliente;
            reserva.FechaInicio = Calendar1.SelectedDate;
            reserva.Habitacion = db.Habitacion.Find(idHabitacion);
            reserva.NumeroNoches = (Calendar2.SelectedDate.DayOfYear - Calendar1.SelectedDate.DayOfYear);

            reserva.Fecha = hoy;

            PagoReserva pago = new PagoReserva();
            pago.Reserva = reserva;
            db.Reserva.Add(reserva);
            pago.Pago = (((reserva.NumeroNoches * Convert.ToInt32(reserva.Habitacion.Precio)) * descuentoCliente) / 100);
            db.PagoReserva.Add(pago);
            db.SaveChanges();
            Response.Redirect("MenuCliente.aspx");
        }
        private Boolean comprobarFechas()
        {
            DateTime inicio = Calendar1.SelectedDate;
            DateTime fin = Calendar2.SelectedDate;
            DateTime hoy = DateTime.Now;

            if (inicio != null && fin != null)
            {
                if (inicio >= hoy && fin > inicio)
                {
                    return true;
                }
            }
            return false;
        }
        private void comprobarSesion()
        {
            Usuario user = Session["conectado"] as Usuario;
            if (user != null)
            {
                if (!user.TipoUsuario.Nombre.Equals("CLIENTE"))
                {
                    Response.Redirect("Default.aspx");
                }
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        private void consultarHabitacionesDisponibles()
        {
            if (comprobarFechas())
            {
                var db = new DB();
                List<Habitacion> habitaciones = db.Habitacion.ToList();
                List<Habitacion> habitacionesDisponibles = new List<Habitacion>();
                foreach (Habitacion h in habitaciones)
                {
                    Boolean disponible = true;
                    List<Reserva> reservas = h.Reserva.ToList();
                    foreach (Reserva r in reservas)
                    {
                        DateTime inicio = r.FechaInicio;
                        DateTime fin = r.FechaInicio.AddDays(r.NumeroNoches);
                        if ((Calendar1.SelectedDate >= inicio && Calendar1.SelectedDate <= fin) || (Calendar2.SelectedDate >= inicio && Calendar2.SelectedDate <= fin))
                        {
                            disponible = false;
                            break;
                        }
                    }
                    if (disponible)
                    {
                        habitacionesDisponibles.Add(h);
                    }
                }
                LlenarDropListHabitacion(habitacionesDisponibles);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Comprueba las fechas!')", true);
            }




        }

        protected void Calendar2_SelectionChanged(object sender, EventArgs e)
        {
            consultarHabitacionesDisponibles();
        }
    }
}