private
var revertFogState = false;

function Start() {
  var aspect = Screen.width / Screen.height;
  var height = .25f;
  var width = height / aspect;
  var rect = new Rect(1 - width - 0.01, 1 - height - 0.01, width, height);
  gameObject.GetComponent(Camera).rect = rect;
}

function OnPreRender() {
  revertFogState = RenderSettings.fog;
  RenderSettings.fog = false;
}

function OnPostRender() { RenderSettings.fog = revertFogState; }

@script RequireComponent(Camera)
