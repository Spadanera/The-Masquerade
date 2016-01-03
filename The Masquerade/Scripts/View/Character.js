function setValue(index, value) {
    $("input[name='[" + index + "].ActualValue']").val(value);
    var radio = $("." + index);
    for (var i = 0; i < radio.length; i++) {
        if (Number(radio[i].value) <= Number(value)) {
            radio[i].checked = true;
        }
        else {
            radio[i].checked = false;
        }
    }
}

function resetValue(index) {
    $("input[name='[" + index + "].ActualValue']").val('0');
    var radio = $("." + index);
    for (var i = 0; i < radio.length; i++) {
        radio[i].checked = false;
    }
}

function setPath(value) {
    //$("#path option[value='" + value + "']").attr("selected", "selected");
    $("#Path option[value='" + value + "']").attr("selected", "selected");
}

function setValueWP(index, value) {
    var indexTemp = (Number(index) + 1).toString();
    var check = $("." + indexTemp);
    for (var i = value; i < check.length; i++) {
        check[i].checked = false;
    }

    setValue(index, value);
}

function AddBlock(target) {
    var maxLength = Number($("#maxLength").val());
    var Disciplines = [
        "Animalism",
        "Assamite Sorcery",
        "Auspex",
        "Celerity",
        "Chimerstry",
        "Daimoinon",
        "Dark Thaumaturgy",
        "Dementation",
        "Dominate",
        "Flight",
        "Fortitude",
        "Koldunic Sorcery",
        "Melpominee",
        "Mortis",
        "Mytherceria",
        "Necromancy",
        "Nihilistics",
        "Obeah",
        "Obfuscate",
        "Obtenebration",
        "Potence",
        "Presence",
        "Protean",
        "Quietus",
        "Sanguinus",
        "Serpentis",
        "Setite Sorcery",
        "Temporis",
        "Thanatosis",
        "Thaumaturgy",
        "Thaumaturgical Countermagic",
        "Valeren",
        "Vicissitue",
        "Visceratika"
    ];
    var Backgrounds = [
        "Allies",
        "Alternate Identity",
        "Black Hand Membership",
        "Contanct",
        "Domain",
        "Fame",
        "Generation",
        "Herd",
        "Influence",
        "Mentor",
        "Resources",
        "Retainers",
        "Rituals",
        "Status",
        "Mentor"
    ];

    if (target == "Disciplines") {
        var Target = Disciplines;
    } else {
        var Target = Backgrounds;
    }

    var content = '<div class="form-group" id="' + maxLength + '_Block">' +
            '<select name="[' + maxLength + '].Name" id="[' + maxLength + '].Name" class="form-control">' +
            '<option value=""></option>';

    for (j = 0; j < Target.length; j++) {
        content = content + '<option value="' + Target[j] + '">' + Target[j] + '</option>';
    }
    content = content + '</select>' +
        '<input type="button" value="" onclick="deleteBlock(' + maxLength + ')" class="img-btn"/>' +
		'<fieldset id="[' + maxLength + ']">' +
        '<div>';
    for (i = 1; i <= 5; i++) {
        content = content + '<input class="' + maxLength + '" ondblclick="resetValue(' + maxLength + ')" onclick="javascript: setValue(' + maxLength + ', value)" type="radio" value="' +
            i + '" />';
    }
    content = content + '</div>' +
        '<input type="hidden" name="[' + maxLength + '].Section" id="[' + maxLength + '].Section" value="' + target + '" />' +
        '<input type="hidden" name="[' + maxLength + '].MaxValue" id="[' + maxLength + '].MaxValue" value="5" />' +
        '<input type="hidden" name="[' + maxLength + '].ActualValue" id="[' + maxLength + '].ActualValue" value="0" />' +
        '</div></div>';

    if (target == "Disciplines") {
        $("#Disciplines").append(content);
    } else {
        $("#Backgrounds").append(content);
    }
    maxLength = maxLength + 1;
    $("#maxLength").val(maxLength);
}

function deleteBlock(index) {
    $("input[name='[" + index + "].Section']").val("Deleted");
    $("#" + index + "_Block").css("display", "none");
}

function AddBlockMF(target) {
    var maxLength = Number($("#maxLength").val());
    var content = '<div class="form-group" id="' + maxLength + '_Block">';
    content = content + '<input class="form-control text-box single-line" type="text" id ="[' + maxLength + ']_Name" name="[' + maxLength + '].Name" />' +
        '<span class="field-validation-valid text-danger" data-valmsg-for="[' + maxLength + '].Name" data-valmsg-replace="true"></span>' +
        '<input type="number" class="form-control text-box single-line" data-val="true" data-val-number="The field Cost must be a number." data-val-required="Cost field is required." name="[' + maxLength + '].ActualValue" id="[' + maxLength + '].ActualValue" value="0" />' +
        '<input type="button" value="" onclick="deleteBlock(' + maxLength + ')" class="img-btn"/>' +
        '<input type="hidden" name="[' + maxLength + '].Section" id="[' + maxLength + '].Section" value="' + target + '" />' +
        '<input type="hidden" name="[' + maxLength + '].MaxValue" id="[' + maxLength + '].MaxValue" value="0" />' +
        '</div>';

    if (target == "Merits") {
        $("#Merits").append(content);
    } else {
        $("#Flaws").append(content);
    }
    maxLength = maxLength + 1;
    $("#maxLength").val(maxLength);
}

function scrollResize() {
    $(document).scroll(function () {
        if ($(window).width() >= 1200) {
            if ($(this).scrollTop() > 10) {
                $(".navbar-bottom").css("cssText", "margin-top: 32px !important");
                $("#main-content").css({ "margin-top": "45px" });
                //$("#logo").css({ "content": "url('../../Content/Images/banner-alt.jpg')" });
                $("#logo").prop("src", "../../Content/Images/banner-alt.jpg");
                $(".navbar").css("cssText", "max-height: 82px !important");
                $("div .extra-left").css("cssText", "margin-left: -213px !important" );
                $("div > img.resize").css({ "height": "82px" });
                $("#banner-inner-left").css({ "height": "82px" });
                $("#banner-inner-left").css({ "margin-right": "173px" });
            }
        }
        //else {
        //    $("#navbar-main").hide();
        //    $("h2").css("cssText", "margin-top: -40px !important");
        //}

        //if ($(this).scrollTop() < 10 && $(window).width() < 768) {
        //    $("#navbar-main").show();
        //    $("h2").css("cssText", "margin-top: 20px !important");
        //}
    });
}