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
        let course = 0;
        let option = "0";
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
            option += "," + String($(this).val());
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
            if (currentMonth == -1) {
                currentMonth = 11;
                currentYear--;
            }
            renderCalendar(currentYear, currentMonth);

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
            renderWeeker(currentYear, currentMonth, currentDay);

        }


    });

    $("#nextBtn,#next").mousedown("click", function () {
        if (count) {
            currentMonth++;
            if (currentMonth == 12) {
                currentMonth = 0;
                currentYear++;
            }
            renderCalendar(currentYear, currentMonth);

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
            renderWeeker(currentYear, currentMonth, currentDay);


        }

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
    var weekBody = $("#weekBody");
    weekBody.empty();

    const cell = $("<div>").addClass("day-week").html("");
    weekBody.append(cell);

    month++;
    $("#weekBody").val(new Date(year, month - 1, date));
    for (let i = 0; i < 7; i++) {
        if (date > totalDays) {
            date = 1;
            month++;
            if (month > 12) month = 1,year++;
        }
        if (firstDay > 6) firstDay = 0;
        const cell = $("<div>").addClass("day-week").html(month + " / " + date + "&#13;" + "<br>(" + weekdays[firstDay] + ")");
        cell.val(new Date(year, month - 1, date));
        let values = $(".s_date").map(function () {
            return $(this).val();
        }).get();
        for (let j = 0; j < values.length; j++) {
            const formattedDate = year + '-' + (month < 10 ? '0' : '') + month + '-' + (date < 10 ? '0' : '') + date;      
            let ren = values[j];
            if (currentDate.getFullYear() > year || (currentDate.getFullYear() == year && currentDate.getMonth() > month - 1) || (currentDate.getMonth() == month - 1 && currentDate.getFullYear() == year && currentDate.getDate() > date)) $(`#date_${i}-id_${ren}`).html("");
            else  getstatus(values[j], formattedDate, i);
        }
        date++;
        firstDay++;
        weekBody.append(cell);
    }
    
    $(".day-cell").on("click", function () {
        let values = $(".day-week").map(function () {
            return $(this).val();
        }).get();
        let selectDate = parseInt($(this).find('.selectDate').val());
        let select_id = parseInt($(this).find('.select_id').val());
        let date1 = values[8 + selectDate];
        const formattedDate = date1.getFullYear() + '-' + (date1.getMonth() < 9 ? '0' : '') + (date1.getMonth() + 1) + '-' + (date1.getDate() < 10 ? '0' : '') + date1.getDate();

        $("#hidden1").val(formattedDate);
        $("#modal_select").val(select_id);
        if ($(`#date_${selectDate}-id_${select_id}`).html() == "●")  $("#modal_form").submit();
    });

}

async function renderCalendar(year, month) {
    const monthYear = $("#monthYear");
    const calendarBody = $("#calendarBody");

    monthYear.html(`${currentYear} 年 ${currentMonth+1}月 `);
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
        let cell;
        let res = await Status(i, (currentMonth + 11) % 12, year);
        if (currentDate.getFullYear() > year || (currentDate.getFullYear() == year && currentDate.getMonth() > (month + 11) % 12) || (currentDate.getMonth() == (month + 11) % 12 && currentDate.getFullYear() == year && currentDate.getDate() > i)) cell = $("<button>").addClass("day-cell text-muted").html(i);
        else if (res) cell = $("<button>").addClass("day-cell text-muted").html(i + "<br>" +"x");
        else cell = $(`<button onclick='selectDate(${i},${(month + 11) % 12 + 1},${year})'>`).addClass("day-cell text-muted").html(i + "<br>" + "●");
        cell.val(i*100+(month+11)%12+1);
        calendarBody.append(cell);
    }

    for (var i = 1; i <= totalDays; i++) {
        let cell;
        let res = await Status(i, currentMonth, year);
        if (currentDate.getFullYear() > year || (currentDate.getFullYear() == year && currentDate.getMonth() > month) || (currentDate.getMonth() == month && currentDate.getFullYear() == year && currentDate.getDate() > i)) cell = $("<button>").addClass("day-cell text-muted").html(i);
        else if (res) cell = $("<button>").addClass("day-cell").html(i + "<br>" + "x");
        else cell = $(`<button onclick='selectDate(${i}, ${month + 1},${year})'>`).addClass("day-cell").html(i + "<br>" + "●");
  
        cell.val(i * 100 + month + 1);
        calendarBody.append(cell);
    }

    for (let i = 1; i <= remainingDays; i++) {
        let res = await Status(i, (currentMonth + 1) % 12, year);
        let cell;
        if (currentDate.getFullYear() > year || (currentDate.getFullYear() == year && currentDate.getMonth() > (month + 1) % 12) || (currentDate.getMonth() == (month + 1) % 12 && currentDate.getFullYear() == year && currentDate.getDate() > i)) cell = $("<button>").addClass("day-cell text-muted").html(i);
        else if (res) cell = $("<button>").addClass("day-cell text-muted").html(i + "<br>" + "×");
        else cell = $(`<button onclick='selectDate(${i},${(month + 1) % 12 + 1},${year})'>`).addClass("day-cell text-muted").html(i + "<br>" + "●");
        cell.val(i*100+(month+1)%12+1);
        calendarBody.append(cell);
    }
}

async function Status(day, month, year) {
    var S_Date = new Date(year, month, day);
    let data = 0;
    await a();

    async function a() {
        const k = S_Date.getFullYear() + '-' + (S_Date.getMonth() < 9 ? '0' : '') + (S_Date.getMonth() + 1) + '-' + (S_Date.getDate() < 10 ? '0' : '') + S_Date.getDate();
        const response = await fetch("/Home/GetReservation", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'  // Add the Content-Type header
            },
            body: JSON.stringify(k)
        });
        data = await response.json();
    }

    return data;
}
function selectDate(i, month, year) {
    let selectyear = year;
    if (currentMonth == 11 && month == 1) selectyear++;
    const date = new Date(selectyear, month - 1, i);

    const dayOfWeek = date.getDay();

    $("#selectdate").html(selectyear + "年" + month + "月" + i + "日" + "(" + weekdays[dayOfWeek] + ")");

    openModal(selectyear, month, i, weekdays[dayOfWeek]);
    // $(".savebtn").on("click", function () {
    //   closeModal(dayCell);
    // });
}

async function openModal(year, month, date, day) {
    //   $("#modal-container").css("display", "block");
    $('#exampleModal').modal('show');
    let date1 = new Date(year, month - 1, date);
    const formattedDate = date1.getFullYear() + '-' + (date1.getMonth() < 9 ? '0' : '') + (date1.getMonth() + 1) + '-' + (date1.getDate() < 10 ? '0' : '') + date1.getDate();

    $("#hidden1").val(formattedDate);
    let values = $(".s_date").map(function () {
        return $(this).val();
    }).get();
    for (let i = 0; i < values.length; i++) {
        getstatus(values[i], formattedDate,0);
    }
    $(".s_coursem").on("click", function () {   
        let selectDate = parseInt($(this).find('.s_date').val());
        $("#modal_select").val(selectDate);
        if ($(`#reservationStatus_${selectDate}`).html() =="●") $("#modal_form").submit();
    });
}
async function getstatus(number, formattedDate,index) {
    const data = {
        Date: formattedDate,
        Number: number
    }
    const response = await fetch("/Home/GetStatus", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'  // Add the Content-Type header
        },
        body: JSON.stringify(data)
    });
    let m = await response.json();
    if (m) {
        $(`#date_${index}-id_${number}`).html("×");
        $(`#reservationStatus_${number}`).html("×");
    }
    else {
        $(`#reservationStatus_${number}`).html("●");
        $(`#date_${index}-id_${number}`).html("●");
    }

}
async function a(S_Date) {

    const response = await fetch("/Home/GetReservation", {
        method: "POST",
        headers: {
            'Content-Type': 'application/json'  // Add the Content-Type header
        },
        body: JSON.stringify(S_Date)
    });
    const data = await response.json();
    return data;
}

function pay_click() {
    
    if ($("#h_course_id").val() > 0)  $("#pay_form").submit();
}

function reservation() {
    let values = $(".phone").map(function () {
        return $(this).val();
    }).get();
    $("#phonenumber").val(parseInt(values[0] + values[1] + values[2]))
    const update = currentDate.getFullYear() + '-' + (currentDate.getMonth() < 9 ? '0' : '') + (currentDate.getMonth() + 1) + '-' + (currentDate.getDate() < 10 ? '0' : '') + currentDate.getDate();
    $("#update").val(update);
    if ($("#name").val() && values[0].length == 3 && values[1].length == 3 && values[2].length == 3 && $("#email").val() && $("#verfy").val()) {
        if ($("#email").val() == $("#verfy").val()) 
            $(".back-system").submit();
    }
}
