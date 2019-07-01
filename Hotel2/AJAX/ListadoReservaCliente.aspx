﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListadoReservaCliente.aspx.cs" Inherits="Hotel2.AJAX.ListadoReservaCliente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table class="table table-stripped">
        <thead>
            <tr>
                <th>Fecha</th>
                <th>Fecha Inicio</th>
                <th>Numero de Noches</th>
                <th>Habitacion</th>
                <th>Tipo de Habitacion</th>
                <th>Cantidad Personas</th>
            </tr>
        </thead>
        <tbody id="reservas">
        </tbody>
    </table>
    

    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@8"></script>
    <script>
        $(document).ready(function (e) {


            Swal.fire({
                type: 'success',
                title: 'Por favor espere',
                showConfirmButton: false,
            });
            
            //ahora haremos la llamada ajax
            $.ajax({
                method: "POST",
                url: "http://localhost:52853/AjaxAPI?f=listado_reserva_cliente",
                data: {}
            }).done(function (respuesta) {
                Swal.close();
                var jsonObj = JSON.parse(respuesta);
                var reserva = jsonObj.reservas;
                var tipo = jsonObj.tipo;
                var habitaciones = jsonObj.habitacion;
                console.log(habitaciones[0].Descripcion);

                $.each(reserva, function (i, reservas) {
                    console.log(reservas.TipoHabitacionId);
                    console.log(reservas.HabitacionId);

                    var descripcionHabi = "";
                    var tipoNombre = "";

                    for (var i = 0; i < habitaciones.length; i++) {
                        if (habitaciones[i].Id == reservas.HabitacionId) {
                            descripcionHabi = habitaciones[i].Descripcion;
                        }
                    }
                    for (var i = 0; i < tipo.length; i++) {
                        if (tipo[i].Id == reservas.TipoHabitacionId) {
                            tipoNombre = tipo[i].Nombre;
                        }
                    }
                    var fila =
                        "<tr>" +
                        "   <td>" + reservas.Fecha + "</td>" +
                        "   <td>" + reservas.FechaInicio + "</td>" +
                        "   <td>" + reservas.NumeroNoches + "</td>" +
                        "   <td>" + descripcionHabi + "</td>" +
                        "   <td>" + tipoNombre + "</td>" +
                        "   <td>" + reservas.CantidadPersonas + "</td>" +
                        "   <td><div class='btn btn-danger eliminar' reserva_id='" + reservas.Id + "'>Eliminar</div></td>" +
                        "   <td><a href='calificacion.aspx?reserva_id='" + reservas.Id + "'>Calificar</a></td>"
                        "</tr>";

                    $("#habitaciones").append(fila);
                });
                $.('.eliminar').click(function (e) {
                    var boton = $(this);
                    var reserva_id = boton.attr('reserva_id');
                    $.ajax({
                        method: "POST",
                        url: "http://localhost:52853/AjaxAPI?f=eliminar_reserva_cliente",
                        data: { 'reserva_id': reserva_id, }
                    }).done(function (respuesta) {
                        var jsonObj = JSON.parse(respuesta);
                        var resultado = jsonObj.mensaje;
                        if (resultado == "OK") {
                            boton.parent().parent().remove();
                        }
                        else {
                            Swal.fire({
                                type: 'error',
                                title: 'Error: ' + resultado,
                            });
                        }
                    });
                });
            });
        });
    </script>
</asp:Content>
