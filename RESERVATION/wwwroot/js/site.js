﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let currentDate = new Date();
let currentYear = currentDate.getFullYear();
let currentMonth = currentDate.getMonth();
let currentDay = currentDate.getDate();
let count = 1;
$(document).ready(function () {
    $('input[type="radio"]').change(function () {
        calculateSum();
    });

    $('input[type="checkbox"]').change(function () {
        calculateSum();
    });
    
    function calculateSum() {
        var radioSum = 0;
        var checkboxSum = 0;
        let course = 0;
        let option = "";
        $('input[type="radio"]:checked').each(function () {
            let values = $(".course_price").map(function () {
                return $(this).val();
            }).get();

            radioSum = values[$(this).val() - 1];
            course += parseInt($(this).val());
        });

        $('input[type="checkbox"]:checked').each(function () {
            let values = $(".option_price").map(function () {
                return $(this).val();
            }).get();

            checkboxSum +=parseInt(values[$(this).val() - 1]);
            option += String($(this).val()+",");
        });

        var totalSum = parseInt(radioSum) + checkboxSum;


        if (totalSum >= 1000) $('#result').text(parseInt(totalSum / 1000) + ',' + parseInt(totalSum % 1000 / 100) + parseInt(totalSum % 100 / 10) + parseInt(totalSum % 10));
        else $('#result').text(totalSum);
        
        $("#h_course_id").val(course);
        $("#h_option_id").val(option);
        $("#h_price").val(totalSum);
  
    }
    renderCalendar(currentYear, currentMonth);
    show();

    $("#month").click(function () {
        show();
        count = 1;

        $("#this").text("今月");

        $("#calendarBody").css("display", "grid");
        $("#weekBody").css("display", "none");
        $("#weekBody1").css("display", "none");

        $(".header").css("display", "flex");
        $(this).css("background-color", "rgba(255,0,0,0.5)")
        $("#week").css("background-color", "rgba(255,255,255,0.5)");

    })
    renderWeeker(currentYear, currentMonth, currentDay);

    $("#week").click(function () {
        show();
        count = 0;
        $("#this").html("今週");

        $("#calendarBody").css("display", "none");
        $("#weekBody").css("display", "grid");
        $("#weekBody1").css("display", "grid");

        $(".header").css("display", "none");
        $(this).css("background-color", "rgba(255,0,0,0.5)");
        $("#month").css("background-color", "rgba(255,255,255,0.5)");


    })

});

let weekdays = ["日", "月", "火", "水", "木", "金", "土"];

function show() {
    $("#prevBtn,#prev").on("click", function () {
        if (count) {
            currentMonth--;
        } else {
            const totalday = new Date(currentYear, currentMonth, 0).getDate();
            currentDay -= 7;
            if (currentDay <= 0) {
                currentMonth--;
                currentDay = totalday + currentDay;
            }
            if (currentMonth < 0) {
                currentMonth = 11;
                currentYear--;
            }

        }
        renderCalendar(currentYear, currentMonth);

        renderWeeker(currentYear, currentMonth, currentDay);

    });

    $("#nextBtn,#next").on("click", function () {
        if (count) {
            currentMonth++;
        } else {
            const totalday = new Date(currentYear, currentMonth + 1, 0).getDate();
            currentDay += 7;
            if (currentDay > totalday) {
                currentDay -= totalday;
                currentMonth++;
            }
            if (currentMonth > 11) {
                currentMonth = 0;
                currentYear++;
            }

        }
        console.log(count)
        renderCalendar(currentYear, currentMonth);

        renderWeeker(currentYear, currentMonth, currentDay);
    });

    $("#this").on("click", function () {
        currentMonth = currentDate.getMonth();
        currentYear = currentDate.getFullYear();
        currentDay = currentDate.getDate();
        renderCalendar(currentYear, currentMonth);
        renderWeeker(currentYear, currentMonth, currentDay);

    })
}
function renderWeeker(year, month, date) {
    let firstDay = new Date(year, month, date).getDay();
    const totalDays = new Date(year, month + 1, 0).getDate();
    const weekBody = $("#weekBody");
    weekBody.empty();

    const cell = $("<div>").addClass("day-week").html("");
    weekBody.append(cell);

    month++;

    for (let i = 0; i < 7; i++) {
        if (date > totalDays) {
            date = 1;
            month++;
            if (month > 12) month = 1,year++;
        }
        if (firstDay > 6) firstDay = 0;
        const cell = $("<div>").addClass("day-week").html(month + "/" + date + "&#13;" + "<br>(" + weekdays[firstDay] + ")");
        cell.val(new Date(year, month - 1, date));
        date++;
        firstDay++;
        weekBody.append(cell);
    }



    $(".day-cell").on("click", function () {

        if (count) selectDate($(this), currentMonth, currentYear);
        else {
            
            let values = $(".day-week").map(function () {

                return $(this).val();
            }).get();
            let selectDate = parseInt($(this).find('.selectDate').val());
            let select_id = parseInt($(this).find('.select_id').val());
            let date1 = values[8 + selectDate];
            const formattedDate = date1.getFullYear() + '-' + (date1.getMonth() < 9 ? '0' : '') + (date1.getMonth() + 1) + '-' + (date1.getDate() < 10 ? '0' : '') + date1.getDate();
            $("#hidden1").val(formattedDate);
            $("#modal_select").val(select_id);
           
            $("#modal_form").submit();
        }

    });

}

function renderCalendar(year, month) {
    const monthYear = $("#monthYear");
    const calendarBody = $("#calendarBody");

    monthYear.html(`${year} 年 ${new Date(year, month).toLocaleString('default', { month: 'numeric' })}月 `);
    calendarBody.empty();

    const firstDay = new Date(year, month, 1).getDay();
    const totalDays = new Date(year, month + 1, 0).getDate();
    
    $("#prevBtn").html((currentMonth + 11) % 12 + 1 + "月");
    $("#nextBtn").html((currentMonth + 1) % 12 + 1 + "月");

    const prevMonth = month === 0 ? 11 : month - 1;
    const prevMonthYear = prevMonth === 11 ? year - 1 : year;
    const totalPrevDays = new Date(prevMonthYear, prevMonth + 1, 0).getDate();
    const startFrom = totalPrevDays - firstDay + 1;
    var remainingDays = 42 - (totalDays + firstDay);

    if (remainingDays >= 7) {
        remainingDays -= 7;
    }

    for (let i = 0; i < 7; i++) {
        const cell = $("<div>").addClass("day-week").text(weekdays[i]);
        calendarBody.append(cell);
    }

    for (let i = startFrom; i <= totalPrevDays; i++) {
        const cell = $("<button>").addClass("day-cell text-muted").text(i);
        cell.val((currentMonth + 11) % 12 + 1);
        calendarBody.append(cell);
    }

    for (let i = 1; i <= totalDays; i++) {
        const cell = $("<button>").addClass("day-cell").text(i);
        cell.val(currentMonth + 1);
        calendarBody.append(cell);
    }

    for (let i = 1; i <= remainingDays; i++) {
        const cell = $("<button>").addClass("day-cell text-muted").text(i);
        cell.val((currentMonth + 1) % 12 + 1);
        calendarBody.append(cell);
    }
}


function selectDate(dayCell, currentMonth, currentYear) {
    let selectyear = currentYear;
    if (currentMonth == 11 && dayCell.val() == 1) selectyear++;
    const date = new Date(selectyear, dayCell.val() - 1, dayCell.text());

    const dayOfWeek = date.getDay();
    $("#selectdate").html(selectyear + "年" + (dayCell.val()) + "月" + dayCell.text() + "日" + "(" + weekdays[dayOfWeek] + ")");
    openModal(selectyear, dayCell.val(), dayCell.text(), weekdays[dayOfWeek]);
    // $(".savebtn").on("click", function () {
    //   closeModal(dayCell);
    // });
}

function openModal(year, month, date, day) {
    //   $("#modal-container").css("display", "block");
    $('#exampleModal').modal('show');
    let date1 = new Date(year, month - 1, date);
    const formattedDate = date1.getFullYear() + '-' + (date1.getMonth() < 9 ? '0' : '') + (date1.getMonth() + 1) + '-' + (date1.getDate() < 10 ? '0' : '') + date1.getDate();

     $("#hidden1").val(formattedDate);

    $("#modal_select").on("change", function () {
        console.log($("#modal_form").html());
        $("#modal_form").submit();
    });
    const time = year + '/' + month + '/' + date + '(' + day + ')';
}

function pay_click() {
    
    if ($("#h_course_id").val() > 0)  $("#pay_form").submit();
}

function reservation() {
    let values = $(".phone").map(function () {
        return $(this).val();
    }).get();
    $("#phonenumber").val(parseInt(values[0] + values[1] + values[2]))
    const update = currentYear + '-' + (currentMonth < 9 ? '0' : '') + (currentMonth + 1) + '-' + (currentDay < 10 ? '0' : '') + currentDay;
    $("#update").val(update);
    if ($("#name").val() && values[0].length == 3 && values[1].length == 3 && values[2].length == 3 && $("#email").val() && $("#verfy").val()) {
        if ($("#email").val() == $("#verfy").val()) $(".back-system").submit();
    }
}
// function closeModal(dayCell) {
//   dayCell.addClass("selected-date");
//   $("#modal-container").css("display", "none");
// }