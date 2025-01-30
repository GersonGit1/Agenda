document.addEventListener('DOMContentLoaded', function () {
    //mostrar las tareas en el calendario
    var calendarEl = document.getElementById('calendar');
    function ObtenerFechaLocal(fecha) {
        const dia = fecha.getDate();
        const mes = fecha.getMonth() >= 10 ? (fecha.getMonth() + 1) : "0" + (fecha.getMonth() + 1);
        const año = fecha.getFullYear();
        const horas = fecha.getHours() <= 9 ? "0" + fecha.getHours() : fecha.getHours();
        const minutos = fecha.getMinutes() <= 9 ? "0" + fecha.getMinutes() : fecha.getMinutes();;
        const segundos = fecha.getSeconds() <= 9 ? "0" + fecha.getSeconds() : fecha.getSeconds();;
        return fecha = dia + "-" + mes + "-" + año + " " + horas + ":" + minutos + ":" + segundos;
    }
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'dayGridMonth', //dayGridMonth
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        events: '/Tarea/Tareas', // Ruta para cargar las tareas desde tu backend.
        eventClick: function (info) {
            if (info.event.extendedProps.descripcion) {
                Swal.fire({
                    title:info.event.title,
                    icon: "info",
                    text: info.event.extendedProps.descripcion,
                    draggable: true
                });
            }
        }, 
        editable: true, //permite mover los eventos
        eventDrop: function(info){
        //guardar los nuevos datos y enviarlos al servidor
            var eventoActualizado = {
                id: info.event.id,
                Start:  ObtenerFechaLocal( info.event.start),
                End: info.event.end ?  ObtenerFechaLocal(info.event.end) : null
            };
            
            fetch('/Tarea/ActualizarFechas', {
                method: 'POST',
                headers: { 'Content-Type':'application/json' },
                body: JSON.stringify(eventoActualizado)
            }).then(response => {
                if (!response.ok) {
                    alert("Error al actualizar el evento");
                    info.revert();//revertir cambios si falla la actualización
                }
            });
        },
        eventResize: function (info) {
            var eventoActualizado = {
                id: info.event.id,
                Start: ObtenerFechaLocal(info.event.start),
                End: info.event.end ? ObtenerFechaLocal(info.event.end) : null
            };

            fetch('/Tarea/ActualizarFechas', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(eventoActualizado)
            }).then(response => {
                if (!response.ok) {
                    alert("Error al actualizar el evento");
                    info.revert();//revertir cambios si falla la actualización
                }
            });

        }
    });
    calendar.render();

    
});