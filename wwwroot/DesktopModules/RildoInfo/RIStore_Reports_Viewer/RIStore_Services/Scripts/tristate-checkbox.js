/* 
 --- Tristate Checkbox ---
v 0.9.2 19th Dec 2008
By Shams Mahmood
http://shamsmi.blogspot.com 
*/
var STATE_NONE = 0; var STATE_SOME = 1; var STATE_ALL = 2; var UNCHECKED_NORM = "UNCHECKED_NORM"; var UNCHECKED_HILI = "UNCHECKED_HILI"; var INTERMEDIATE_NORM = "INTERMEDIATE_NORM"; var INTERMEDIATE_HILI = "INTERMEDIATE_HILI"; var CHECKED_NORM = "CHECKED_NORM"; var CHECKED_HILI = "CHECKED_HILI"; var DEFAULT_CONFIG = { UNCHECKED_NORM: "/icons/sigma/Unchecked_16X16_Standard.png", UNCHECKED_HILI: "/icons/sigma/Unchecked_16X16_Standard.png", INTERMEDIATE_NORM: "/icons/sigma/Grant_16X16_Standard.png", INTERMEDIATE_HILI: "/icons/sigma/Grant_16X16_Standard.png", CHECKED_NORM: "/icons/sigma/Deny_16X16_Standard.png", CHECKED_HILI: "/icons/sigma/Deny_16X16_Standard.png" }; function getNextStateFromValue(A) { if (A == STATE_SOME) { return STATE_ALL } if (A == STATE_ALL) { return STATE_NONE } return STATE_SOME } function getStateFromValue(B, A) { if (B == STATE_SOME) { return (!A) ? INTERMEDIATE_NORM : INTERMEDIATE_HILI } if (B == STATE_ALL) { return (!A) ? CHECKED_NORM : CHECKED_HILI } return (!A) ? UNCHECKED_NORM : UNCHECKED_HILI } function getFieldAndContainerIds(D) { var E = D.substring(0, D.length - ".Img".length); var C = document.getElementById(E + ".Field").value; var A = document.getElementById(E + ".Container"); var B = ""; if (A) { B = A.value } return [C, B] } function getAllCheckboxesInContainer(C) { if (C == "") { return [] } var B = document.getElementById(C); var G = B.getElementsByTagName("input"); var F = new Array(); var E = 0; for (var A = 0; A < G.length; A++) { var D = G[A]; if (D.type == "checkbox") { F[E++] = D } } return F } function selectOrUnselectBoxes(C, B) { for (var A in C) { C[A].checked = B } } function areAllBoxesInGivenCheckedState(C, D) { var B = true; for (var A = 0; A < C.length; A++) { if (C[A].checked != D) { B = false; break } } return B } function replaceImage(A, C) { var B = document.getElementById(A); if (B.src != C) { B.src = C } } function mouseOverOutOfImage(C, A) { var E = getFieldAndContainerIds(C); var B = document.getElementById(E[0]); var D = getStateFromValue(B.value, A); return DEFAULT_CONFIG[D] } function onMouseOverImage(A) { return function () { var B = mouseOverOutOfImage(A, true); replaceImage(A, B) } } function onMouseOutImage(A) { return function () { var B = mouseOverOutOfImage(A, false); replaceImage(A, B) } } function onTristateImageClick(A, B) { return function () { var F = getFieldAndContainerIds(A); var E = document.getElementById(F[0]); var D = getNextStateFromValue(E.value); if (!B && D == STATE_SOME) { D = getNextStateFromValue(D) } E.value = D; if (F[1] != "") { var C = getAllCheckboxesInContainer(F[1]); selectOrUnselectBoxes(C, D == STATE_ALL) } var G = mouseOverOutOfImage(A, true); replaceImage(A, G) } } function onCheckboxClick(A, B) { return function () { var E = getFieldAndContainerIds(A); var C = getAllCheckboxesInContainer(E[1]); var D = document.getElementById(E[0]); updateStateAndImage(C, D, A) } } function updateStateAndImage(B, D, C) { if (B.length > 0) { var A = areAllBoxesInGivenCheckedState(B, true); var E = areAllBoxesInGivenCheckedState(B, false); if (A) { D.value = STATE_ALL } else { if (E) { D.value = STATE_NONE } else { D.value = STATE_SOME } } } var F = mouseOverOutOfImage(C, false); replaceImage(C, F) } function createHiddenStateField(C, A) { var B = document.createElement("input"); B.id = A; B.type = "hidden"; B.value = STATE_NONE; C.appendChild(B); return B } function createTriStateImageNode(D, C, A) { var B = new Image(); B.id = C; B.src = DEFAULT_CONFIG[UNCHECKED_NORM]; D.appendChild(B); if (D.addEventListener) { D.addEventListener("mouseover", onMouseOverImage(B.id), false); D.addEventListener("mouseout", onMouseOutImage(B.id), false); D.addEventListener("click", onTristateImageClick(B.id, A), false) } else { if (D.attachEvent) { D.attachEvent("onmouseover", onMouseOverImage(B.id)); D.attachEvent("onmouseout", onMouseOutImage(B.id)); D.attachEvent("onclick", onTristateImageClick(B.id, A)) } } } function createFieldNameHiddenField(D, C, B) { var A = document.createElement("input"); A.id = C; A.type = "hidden"; A.value = B; D.appendChild(A) } function createContainerNameHiddenField(D, B, C) { var A = document.createElement("input"); A.id = B; A.type = "hidden"; A.value = C; D.appendChild(A) } function attachOnclickEventsToDependentBoxes(E, D) { var B = getAllCheckboxesInContainer(E); for (var A in B) { var C = B[A]; if (C.addEventListener) { C.addEventListener("click", onCheckboxClick(D, C.id), false) } else { if (C.attachEvent) { C.attachEvent("onclick", onCheckboxClick(D, C.id)) } } } return B } function initTriStateCheckBox(A, G, F) { var D = document.getElementById(A); var C = G; var B = A + ".Value"; if (F) { C = ""; B = G } var I = D.childNodes[0]; D.removeChild(I); var J = document.getElementById(B); if (!F) { J = createHiddenStateField(D, B) } var K = A + ".Img"; createTriStateImageNode(D, K, F); var L = A + ".Field"; createFieldNameHiddenField(D, L, B); if (!F) { var H = A + ".Container"; createContainerNameHiddenField(D, H, C) } D.appendChild(I); var E = attachOnclickEventsToDependentBoxes(C, K); updateStateAndImage(E, J, K) };