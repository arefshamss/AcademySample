let keysPressed = [];
let keysSelected;

let defaultMapping = [
    { "url" : "/Admin/Course/List" , "mappedKey" : "AltLeft+KeyC"} , 
    { "url" : "/Admin/Users/List" , "mappedKey" : "AltLeft+KeyU"} , 
    { "url" : "/Admin/Blogs/List" , "mappedKey" : "AltLeft+KeyM"} ,
    { "url" : "/Admin/SiteSetting/ChangeSiteSetting" , "mappedKey" : "AltLeft+KeyS"} ,
]

class KeyMappings {
    constructor(dbName) {
        this.db = new Dexie(dbName);
        this.db.version(1).stores({
            keybindings: "++id , url , mappedKey"
        });
    }

    async anyAsync(url) {
        try {
            const item = await this.db.keybindings.where('url').equals(url).first();
            return item !== undefined;
        } catch (err) {
            console.error(err);
        }
    }

    async anyKeyMapAsync(mappedKey) {
        try {
            const item = await this.db.keybindings.where('mappedKey').equals(mappedKey).first();
            return item !== undefined;
        } catch (err) {
            console.error(err);
        }
    }

    async getKeyMapByUrl(url) {
        try {
            return await this.db.keybindings.where("url").equals(url).first();
        } catch (err) {
            console.error("failed to find keymap", err);
        }
    }

    async addAsync(url, mappedKey) {
        const defaultMap = defaultMapping.find(s => s.url === url);
        const keyBinding = {
            url: url,
            mappedKey: defaultMap  !== undefined ? defaultMap.mappedKey : mappedKey 
        }
        try {
            if (!await this.anyAsync(url)) {
                
                await this.db.keybindings.add(keyBinding);
            }
        } catch (err) {
            console.error("error adding keybinding : ", err);
        }
    }


  
    async updateAsync(url, mapping) {
        try {
            const item = await this.db.keybindings.where('url').equals(url).first();
            if (item === undefined) {
                return;
            }
            item.mappedKey = mapping;
            await this.db.keybindings.update(item.id, item);
        } catch (err) {
            console.error("error updating keybinding : ", err);
        }
    }
    

    async getAllAsync() {
        try {
            return await this.db.keybindings.toArray();
        } catch (err) {
            console.error("error getting all keybindings : ", err);
        }
    }

}

const db = new KeyMappings("TopLearn.Keys.Db");


function showChangeMappingDialog(url) {
    const view = $(
        "<div class='row'>" +
        "<div class='col-12'>" +
        "<div class='form-group' id='bindingGroup'>" +
        "<label class='form-label'>کلید میانبر</label>" +
        `<input type='hidden' id='mappingUrl' value='${url}'>` +
        "<div class='d-flex p-2 border border-1 selectKey' style='height: 40px' id='selectedKeys'>انتخاب کنید</div>" +
        "<button class='waves-effect waves-light btn-sm btn btn-primary mt-3' id='selectShortcut'>انتخاب</button>" +
        "</div>" +
        "</div>" +
        "</div>"
    );
    $("#modal-center-sm-body").empty().append(view);

    opSmModal("افزودن کلید میانبر", 1);

    $("#selectShortcut").on("click", () => {
        keysSelected = "";
        $("#selectedKeys").empty();
        captureShortcut();
    });
}

function addKeysUi(keysPressed) {
    const selectedKeys = $("#selectedKeys");
    selectedKeys.empty();
    keysPressed.forEach((val) => {
        selectedKeys
            .append(`<span class="badge badge-secondary mx-2">${val.toLowerCase().replace("key" , "")}</span>`);
    })
}

function captureShortcut() {
    keysPressed = [];
    keysSelected = "";

    const keydownHandler = (e) => {
        e.preventDefault();
        e.stopPropagation();
       
        const key =  e.code;
        if (!keysPressed.includes(key)) {
            keysPressed.push(key);
        }
      
        keysSelected = keysPressed.join("+");
      
        addKeysUi(keysPressed);
    };

    const keyupHandler = (e) => {
        e.preventDefault();
        e.stopPropagation();
        const key = e.code;
        keysPressed = keysPressed.filter((k) => k !== key);
        
        if (keysPressed.length === 0) {
            document.removeEventListener("keydown", keydownHandler);
            document.removeEventListener("keyup", keyupHandler);
        }
    };

    document.addEventListener("keydown", keydownHandler);
    document.addEventListener("keyup", keyupHandler);

    if (!$("#saveBtn").length) {
        const url = $("#mappingUrl").val();
        $("#bindingGroup").append(
            `<button type='button' id='saveBtn' class='waves-effect waves-light btn-sm btn btn-success mt-3 me-2' onclick='saveBinding("${url}")'>ذخیره</button>`
        );
    }
    $("#selectShortcut").text("از نو");
}

function saveBinding(url) {
    db.anyKeyMapAsync(keysSelected).then((isAny) => {
        if (isAny) {
            showSweetAlert("کلید میانبر وجود دارد")
        }
    });
    db.updateAsync(url, keysSelected).then(() => {
        closeSmModal(1);
        const keys = keysSelected.split("+");
        if (keys.length > 0 && keys[0] !== "") {
            $(`[data-seed='${url}'] td:nth-child(3)`).empty()
            keys.forEach(item => {
                $(`[data-seed='${url}'] td:nth-child(3)`).append(`<span class="badge badge-secondary mx-2 keyMap">${item.toLowerCase().replace("key" , "")}</span>`);
            });
            $(`[data-seed='${url}'] td:nth-child(4) .btn-danger`).remove();
            $(`[data-seed='${url}'] td:nth-child(4)`).prepend(`<a class="waves-effect waves-light btn-sm btn btn-danger" onclick="removeBinding('${url}')">
                                                <i class="fa fa-trash"></i>
                                            </a>`);
        }
        showSweetAlert("کلید میانبر با موفقیت تغییر یافت", "success")
        bindKeys();
    });
}

const keyListeners = new Map();


function bindKeys() {
    // Reset previous listeners to avoid duplicates
    unbindAllKeys();

    db.getAllAsync().then((result) => {
        const mappedKeys = result.filter((s) => s.mappedKey !== "");

        mappedKeys.forEach((item) => {
            const requiredKeys = item.mappedKey.split("+").sort();
            let keysPressed = new Set();

            const keydownHandler = (e) => {
                keysPressed.add(e.code);
                const pressedKeys = Array.from(keysPressed).sort();
                if (
                    pressedKeys.length === requiredKeys.length &&
                    pressedKeys.every((key, index) => key === requiredKeys[index])
                ) {
                    e.preventDefault();
                    e.stopPropagation();
                    window.location.href = item.url;
                }
            };

            const keyupHandler = (e) => {
                keysPressed.delete(e.code);
            };

            // Add listeners to the map for this key combination
            keyListeners.set(item.mappedKey, { keydownHandler, keyupHandler });

            document.addEventListener("keydown", keydownHandler);
            document.addEventListener("keyup", keyupHandler);
        });
    });
}

function unbindKey(mappedKey) {
    const listeners = keyListeners.get(mappedKey);
    if (listeners) {
        document.removeEventListener("keydown", listeners.keydownHandler);
        document.removeEventListener("keyup", listeners.keyupHandler);
        keyListeners.delete(mappedKey); 
    }
}

function unbindAllKeys() {
    // Remove all existing listeners
    keyListeners.forEach(({ keydownHandler, keyupHandler }) => {
        document.removeEventListener("keydown", keydownHandler);
        document.removeEventListener("keyup", keyupHandler);
    });
    keyListeners.clear();
  
}
function removeBinding(url){
    Swal.fire({
        title: "آیا از حذف کلید میانبر مطمئن هستید ؟",
        icon: "question",
        iconHtml: "؟",
        confirmButtonText: "بله",
        cancelButtonText: "نه",
        showCancelButton: true,
        showCloseButton: true
    }).then((result) => {
        if(result.isConfirmed){

            db.getKeyMapByUrl(url).then((result) => {
                unbindKey(result.mappedKey);
                db.updateAsync(url , "").then(() => {
                    $(`[data-seed='${url}'] .btn-danger`).remove();
                    $(`[data-seed='${url}'] td:nth-child(3)`).empty();
                    showToaster("حذف کلید میانبر با موفقیت انجام شد" , "success");
                });
            })
        }
    });
}

$(() => {
    
    $("#resetToDefault").click(async () => {
        Swal.fire({
            title: "تنظیمات به حالت اولیه بازگردد ؟",
            icon: "question",
            iconHtml: "؟",
            confirmButtonText: "بله",
            cancelButtonText: "نه",
            showCancelButton: true,
            showCloseButton: true
        }).then((result) => {
            if(result.isConfirmed){
                $("[data-seed]").each(async function () {
                    const url = $(this).attr("data-seed");
                    const defaultMap = defaultMapping.find(s => s.url === url);
                    if(defaultMap !== undefined) 
                    {
                        let keymap = await db.getKeyMapByUrl(url);
                        keymap.mappedKey = defaultMap.mappedKey;
                        await db.updateAsync(url, keymap.mappedKey);
                    }else{
                        await db.updateAsync(url,"");
                    }
                });
               window.location.reload();
            }
        });
    })
    
    $("[data-seed]").each(async function () {
        const url = $(this).attr("data-seed");
        
        await db.addAsync(url, "");
        
        const keymap = await db.getKeyMapByUrl(url);
        console.log(keymap.mappedKey);
        const keys = keymap.mappedKey.toString().split("+");
        if (keys.length > 0 && keys[0] !== "") {
            keys.forEach(item => {
                $(this).find("td:nth-child(3)").append(`<span class="badge badge-secondary mx-2 keyMap">${item.toLowerCase().replace("key" , "")}</span>`);
            });
            $(this).find("td:nth-child(4)").prepend(`<a class="waves-effect waves-light btn-sm btn btn-danger" onclick="removeBinding('${url}')">
                                                <i class="fa fa-trash"></i>
                                            </a>`);
        }
    })
    bindKeys();

})