function getSelectedOperatingSystem(){return $(".os-toggle-select.active").data("os")}$(function(){$(".os-toggle-select").click(function(){var n=$(this).data("os");return $(".os-toggle-select").removeClass("active").attr("aria-selected","false"),$(".os-toggle-select."+n).addClass("active").attr("aria-selected","true"),$(".os-toggle").hide(),$(".os-toggle."+n).show(),n==="windows"?($(".prompt-os-toggle").removeClass("prompt-unix"),$(".prompt-os-toggle").addClass("prompt-windows")):($(".prompt-os-toggle").removeClass("prompt-windows"),$(".prompt-os-toggle").addClass("prompt-unix")),!1})});