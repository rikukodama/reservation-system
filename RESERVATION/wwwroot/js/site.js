// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
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
        $('input[type="radio"]:checked').each(function () {
            radioSum += parseInt($(this).val());

        });

        $('input[type="checkbox"]:checked').each(function () {
            checkboxSum += parseInt($(this).val());
        });

         var totalSum = radioSum + checkboxSum;


        if (totalSum >= 1000) $('#result').text(parseInt(totalSum / 1000) + ',' + parseInt(totalSum % 1000 / 100) + parseInt(totalSum % 100 / 10) + parseInt(totalSum % 10));
        else $('#result').text(totalSum);
  
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
            if (month > 12) month = 1;
        }
        if (firstDay > 6) firstDay = 0;
        const cell = $("<div>").addClass("day-week").html(month + "/" + date + "&#13;" + "<br>(" + weekdays[firstDay] + ")");
        date++;
        firstDay++;
        weekBody.append(cell);
    }



    $(".day-cell").on("click", function () {

        if (count) selectDate($(this), currentMonth, currentYear);

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
    $("#modal_select").click(function () {
        $("#modal_form").submit();
    });
    const time = year + '/' + month + '/' + date + '(' + day + ')';
}

function pay_click() {
    var course = "";
    var option = "";
    $('input[type="radio"]:checked').each(function () {
        course += parseString($(this).text());

    });
    
    $('input[type="checkbox"]:checked').each(function () {
        option += parseString($(this).text());
    });

    $("#h_course_id").val(course);
    $("#h_option_id").val(option);
    $("#pay_form").submit();
}
// function closeModal(dayCell) {
//   dayCell.addClass("selected-date");
//   $("#modal-container").css("display", "none");
// }
