﻿
    $("#porSala").on('change', function () {
        let salaId = $("#porSala").val();
        window.location.href = '/DispSala/HorariosDisponiveis?salaId=' + salaId;
    });
