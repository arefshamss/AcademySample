function onCheckChildPermission(){
    let grandPermission =  $(this).parents(".child-list").prev(".permission-grand");
    let selectedChildCount = 0;
    let totalCount  = $(grandPermission).next(".child-list").find(".permission-child").length;
    $(grandPermission).next(".child-list").find(".permission-child").each(function (){
        if($(this).prop("checked")) {
            selectedChildCount++;
        }
    });
    if(selectedChildCount >= 1){
        $(grandPermission).find(".parent").prop("checked" , true);
    }else{
        $(grandPermission).find(".parent").prop("checked" , false);
    }
}

function onCheckGrandPermission(){
    let children  = $(this).next(".child-list").find(".permission-child");
    if($(this).find(".parent").prop("checked")){
        children.each(function(){
            $(this).prop("checked" , true);
        });
    }else{
        children.each(function(){
            $(this).prop("checked" , false);
        });
    }
}


$(() => {
    let grandPermissions  =  $(".permission-grand");
    let childPermissions = $(".permission-child");
    grandPermissions.each(function(){
        let selectedChildCount = 0;
        let totalCount  = $(this).next(".child-list").find(".permission-child").length;
        $(this).next(".child-list").find(".permission-child").each(function (){

            if($(this).find(".parent") === undefined || $(this) == null)
            {
                return ;
            }
            if($(this).find(".parent").prop("checked")) {
                selectedChildCount++;
            }
        });
        if(totalCount !== 0 && selectedChildCount === totalCount){
            $(this).find(".parent").prop("checked" , true);
        }
    });

    childPermissions.on("change" , onCheckChildPermission);

    grandPermissions.on("change" , onCheckGrandPermission);

})
