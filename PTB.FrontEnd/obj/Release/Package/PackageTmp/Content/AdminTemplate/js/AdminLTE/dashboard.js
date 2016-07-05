/*
 * Author: Abdullah A Almsaeed
 * Date: 4 Jan 2014
 * Description:
 *      This is a demo file used only for the main dashboard (index.html)
 **/

$(function() {
    "use strict";
   
    //Make the dashboard widgets sortable Using jquery UI
    $(".connectedSortable").sortable({
        placeholder: "sort-highlight",
        connectWith: ".connectedSortable",
        handle: ".box-header, .nav-tabs",
        forcePlaceholderSize: true,
        zIndex: 999999
    }).disableSelection();
    $(".connectedSortable .box-header, .connectedSortable .nav-tabs-custom").css("cursor", "move");
    //jQuery UI sortable for the todo list
    $(".todo-list").sortable({
        placeholder: "sort-highlight",
        handle: ".handle",
        forcePlaceholderSize: true,
        zIndex: 999999
    }).disableSelection();
    ;

    //bootstrap WYSIHTML5 - text editor
    $(".textarea").wysihtml5();

    $('.daterange').daterangepicker(
            {
                ranges: {
                    'Today': [moment(), moment()],
                    'Yesterday': [moment().subtract('days', 1), moment().subtract('days', 1)],
                    'Last 7 Days': [moment().subtract('days', 6), moment()],
                    'Last 30 Days': [moment().subtract('days', 29), moment()],
                    'This Month': [moment().startOf('month'), moment().endOf('month')],
                    'Last Month': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
                },
                startDate: moment().subtract('days', 29),
                endDate: moment()
            },
    function(start, end) {
        alert("You chose: " + start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    });

    /* jQueryKnob */
    $(".knob").knob();

    //jvectormap data
    var visitorsData = {
        "US": 398, //USA
        "SA": 400, //Saudi Arabia
        "CA": 1000, //Canada
        "DE": 500, //Germany
        "FR": 760, //France
        "CN": 300, //China
        "AU": 700, //Australia
        "BR": 600, //Brazil
        "IN": 800, //India
        "GB": 320, //Great Britain
        "RU": 3000 //Russia
    };
    //World map by jvectormap
    $('#world-map').vectorMap({
        map: 'world_mill_en',
        backgroundColor: "transparent",
        regionStyle: {
            initial: {
                fill: '#e4e4e4',
                "fill-opacity": 1,
                stroke: 'none',
                "stroke-width": 0,
                "stroke-opacity": 1
            }
        },
        series: {
            regions: [{
                    values: visitorsData,
                    scale: ["#92c1dc", "#ebf4f9"],
                    normalizeFunction: 'polynomial'
                }]
        },
        onRegionLabelShow: function(e, el, code) {
            if (typeof visitorsData[code] != "undefined")
                el.html(el.html() + ': ' + visitorsData[code] + ' new visitors');
        }
    });

    //Sparkline charts
    var myvalues = [1000, 1200, 920, 927, 931, 1027, 819, 930, 1021];
    $('#sparkline-1').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [515, 519, 520, 522, 652, 810, 370, 627, 319, 630, 921];
    $('#sparkline-2').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });
    myvalues = [15, 19, 20, 22, 33, 27, 31, 27, 19, 30, 21];
    $('#sparkline-3').sparkline(myvalues, {
        type: 'line',
        lineColor: '#92c1dc',
        fillColor: "#ebf4f9",
        height: '50',
        width: '80'
    });    

    //The Calender
    $("#calendar").datepicker();

    //SLIMSCROLL FOR CHAT WIDGET
    //$('#chat-box').slimScroll({
    //    height: '250px'
    //});

    /* Morris.js Charts */
    // Sales chart
   
    //var line = new Morris.Line({
    //    element: 'line-chart',
    //    resize: true,
    //    data: [
    //        {y: '2012-1', item1: 2666},
    //        { y: '2012-2', item1: 2778 },
    //        { y: '2012-3', item1: 4912 },
    //        { y: '2012-4', item1: 3767 },
    //        { y: '2012-5', item1: 6810 },
    //        { y: '2012-6', item1: 5670 },
    //        { y: '2012-7', item1: 4820 },
    //        { y: '2012-8', item1: 15073 },
    //        { y: '2012-9', item1: 10687 },
    //        { y: '2012-10', item1: 8432 }
    //    ],
    //    xkey: 'y',
    //    ykeys: ['item1'],
    //    labels: ['Item 1'],
    //    xLabels:"month",
    //    lineColors: ['#000000'],
    //    lineWidth: 2,
    //    hideHover: 'auto',
    //    gridTextColor: "#000000",
    //    gridStrokeWidth: 0.4,
    //    pointSize: 4,
    //    pointStrokeColors: ["#000000"],
    //    gridLineColor: "#000000",
    //    gridTextFamily: "Open Sans",
    //    gridTextSize: 10
    //});

    //$(function () {
    //    $('#line-chart').highcharts({
    //        title: {
    //            text: '',
    //            x: -20 //center
    //        },
    //        subtitle: {
    //            text: '',
    //            x: -20
    //        },
    //        xAxis: {
    //            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
    //                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    //        },
    //        yAxis: {
    //            title: {
    //                text: ''
    //            }
             
               
    //        },
    //        tooltip: {
    //            valueSuffix: ''
    //        },
    //        legend: {
    //            layout: 'vertical',
    //            align: 'right',
    //            verticalAlign: 'middle',
    //            borderWidth: 0,
    //            enabled:false
    //        },
    //        //plotOptions: {
    //        //    series: {
    //        //        marker: {
    //        //            enabled: false
    //        //        }
    //        //    }
    //        //},
    //        series: [{
    //            name: 'Members',
    //            data: [7, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]

    //        },
    //        {
    //            name: 'Threshhold',
    //            data: [17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17],
    //            marker:{enabled:false}
    //        }

    //        ]
    //    });
    //});
   
    //$('#line-chart').highcharts({
    //        chart: {
    //            type: 'line'
    //        },
    //        title: {
    //            text: 'Trending'
    //        },
    //        subtitle: {
    //            text: ''
    //        },
    //        xAxis: {
    //            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    //        },
    //        yAxis: {
             
               
                
               
    //        },
    //        plotOptions: {
    //            line: {
    //                dataLabels: {
    //                    enabled: true
    //                },
    //                enableMouseTracking: false
    //            }
    //        },
    //        series: [{
    //            name: 'Months',
    //            data: [500,512,503,400,500,600,525,517,456,500,400,560]
    //        }]
    //    });
   
   
    //Donut Chart
    //var donut = new Morris.Donut({
    //    element: 'sales-chart',
    //    resize: true,
    //    colors: ["#ff0000", "#FCDD04", "#C8C8C8 "],
    //    data: [
    //        {label: "Over", value: 275},
    //        {label: "At Risk", value: 600},
    //        {label: "Safe", value: 870}
    //    ],
    //    hideHover: 'auto'
    //});


    //$(function () {
    //    $('#sales-chart').highcharts({
    //        chart: {
    //            plotBackgroundColor: null,
    //            plotBorderWidth: 0,
    //            plotShadow: false
           
    //        },
    //        title: {
    //            text: '',
    //            align: 'center',
    //            verticalAlign: 'middle',
    //            y: 10
    //        },
    //        tooltip: {
    //            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
    //        },
    //        plotOptions: {
    //            pie: {
    //                dataLabels: {
    //                    enabled: true,
    //                    distance: -3,
    //                    style: {
    //                        fontWeight: 'bold',
    //                        color: 'black'//,
    //                        //textShadow: '0px 1px 2px black'
    //                    }
    //                },
    //                startAngle: 0,
    //                endAngle: 360,
    //                center: ['52%', '45%']
    //            }
    //        },
    //        colors: ['#CC0404', '#FCDD04', '#F2F2F2'],
    //        series: [{
    //            type: 'pie',
    //            name: 'Members',
    //            innerSize: '80%',
    //            data: [
    //                ['Over', 45.0],
    //                ['At Risk', 26.8],
    //                ['Safe', 28.2]


    //            ]
    //        }]
    //    });
    //});
    /*Bar chart
    var bar = new Morris.Bar({
        element: 'bar-chart',
        resize: true,
        data: [
            {y: '2006', a: 100, b: 90},
            {y: '2007', a: 75, b: 65},
            {y: '2008', a: 50, b: 40},
            {y: '2009', a: 75, b: 65},
            {y: '2010', a: 50, b: 40},
            {y: '2011', a: 75, b: 65},
            {y: '2012', a: 100, b: 90}
        ],
        barColors: ['#00a65a', '#f56954'],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['CPU', 'DISK'],
        hideHover: 'auto'
    });*/
    //Fix for charts under tabs
    $('.box ul.nav a').on('shown.bs.tab', function(e) {
        area.redraw();
        donut.redraw();
    });


    /* BOX REFRESH PLUGIN EXAMPLE (usage with morris charts) */
    //$("#loading-example").boxRefresh({
    //    source: "ajax/dashboard-boxrefresh-demo.php",
    //    onLoadDone: function(box) {
    //        bar = new Morris.Bar({
    //            element: 'bar-chart',
    //            resize: true,
    //            data: [
    //                {y: '2006', a: 100, b: 90},
    //                {y: '2007', a: 75, b: 65},
    //                {y: '2008', a: 50, b: 40},
    //                {y: '2009', a: 75, b: 65},
    //                {y: '2010', a: 50, b: 40},
    //                {y: '2011', a: 75, b: 65},
    //                {y: '2012', a: 100, b: 90}
    //            ],
    //            barColors: ['#00a65a', '#f56954'],
    //            xkey: 'y',
    //            ykeys: ['a', 'b'],
    //            labels: ['CPU', 'DISK'],
    //            hideHover: 'auto'
    //        });
    //    }
    //});

    /* The todo list plugin */
    $(".todo-list").todolist({
        onCheck: function(ele) {
            //console.log("The element has been checked")
        },
        onUncheck: function(ele) {
            //console.log("The element has been unchecked")
        }
    });

    //I will uncomment below if Bill wants to add expand collapse again
    
  
});