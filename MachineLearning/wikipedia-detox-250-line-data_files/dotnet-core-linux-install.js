function getDisplayedLinuxDistroId(){return $("#distroSelect").val()}function getDisplayedLinuxDistroName(){return $("#distroSelect option:selected").text()}$(document).ready(function(){$("#distroSelect").change(function(){var n=$(this).val();$(".instructions").addClass("d-none");$(".instructions").removeClass("visible");$(".instructions."+n).addClass("visible");$(".instructions."+n).removeClass("d-none");$.event.trigger("linuxDistroChanged",n)})});