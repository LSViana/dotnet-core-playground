function loadStep(n){$(".nav-page .step-select").closest("li").removeClass("active");$(".nav-page .step-select.step-select-"+n).closest("li").addClass("active");$(".step").hide();$(".step.step-"+n).show();recordStepLoaded(n)}function recordStepLoaded(n){var i=$(".toc ul.nav .step-select").filter(function(){return $(this).data("step")===n}),r=$(".toc ul.nav").children().index(i.closest("li"))+1,t=function(){ga("send","event","Tutorial","Step Loaded",r+" - "+n)};typeof mscc=="undefined"||mscc.hasConsent()?t():postCookieConsentTasks.push(t)}$(function(){$(".step-select").click(function(){var n=$(this).data("step");return loadStep(n),history.pushState({step:n},"","./"+n),!1});$(window).on("popstate",function(){history.state&&history.state.step&&loadStep(history.state.step)})});