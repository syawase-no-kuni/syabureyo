using UnityEngine;
using System;
using live2d;
using live2d.framework;

[ExecuteInEditMode]
public class SimpleModel : MonoBehaviour
{
    public TextAsset mocFile;
    public TextAsset physicsFile;
    public Texture2D[] textureFiles;

    private Live2DModelUnity live2DModel;
    private EyeBlinkMotion eyeBlink = new EyeBlinkMotion();
    private L2DTargetPoint dragMgr = new L2DTargetPoint();
    private L2DPhysics physics;
    private Matrix4x4 live2DCanvasPos;

    void Start()
    {
        Live2D.init();
        load();
    }


    void load()
    {
        live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

        // Live2Dのレンダーモード変更
        live2DModel.setRenderMode(Live2D.L2D_RENDER_DRAW_MESH_NOW);

        for (int i = 0; i < textureFiles.Length; i++)
        {
            live2DModel.setTexture(i, textureFiles[i]);
        }

        float modelWidth = live2DModel.getCanvasWidth();
        live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

        if (physicsFile != null) physics = L2DPhysics.load(physicsFile.bytes);
    }


    void Update()
    {
        if (live2DModel == null) load();
        live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

        if (!Application.isPlaying)
        {
            live2DModel.update();
            return;
        }

        var pos = Input.mousePosition;
        dragMgr.Set(pos.x / Screen.width * 2 - 1, pos.y / Screen.height * 4 - 1);

        dragMgr.update();

        // 角度XY
        live2DModel.setParamFloat("PARAM_ANGLE_X", dragMgr.getX() * 30);
        live2DModel.setParamFloat("PARAM_ANGLE_Y", dragMgr.getY() * 30);

        // 体X
        live2DModel.setParamFloat("PARAM_BODY_ANGLE_X", dragMgr.getX() * -10);

        // 目玉
        live2DModel.setParamFloat("PARAM_EYE_BALL_X", dragMgr.getX());
        live2DModel.setParamFloat("PARAM_EYE_BALL_Y", dragMgr.getY());

        // アホ毛(上)
        live2DModel.setParamFloat("PARAM_AHOGE_TOP_ROTATE", dragMgr.getY());

        //アホ毛(左上)
        live2DModel.setParamFloat("PARAM_AHOGE_LEFT_ROTATE", dragMgr.getY());

        // 髪揺れ横(-1.0f～1.0f)
        live2DModel.setParamFloat("PARAM_HAIR_SIDE", -dragMgr.getX());

        // 口の開閉(0.0f～1.0f)
        live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", -dragMgr.getY());

        // 松茸の大きさ(0.0f:小～1.0f:大)
        live2DModel.setParamFloat("PARAM_MATSUTAKE_SCALE", 1.0f);

        //涙の大きさ(0.0f:小～1.0f:大)
        live2DModel.setParamFloat("PARAM_NAMIDA_SCALE_RIGHT", 1.0f);
        live2DModel.setParamFloat("PARAM_NAMIDA_SCALE_LEFT", 1.0f);


        double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
        double t = timeSec * 2 * Math.PI;
        live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));

        eyeBlink.setParam(live2DModel);

        if (physics != null) physics.updateParam(live2DModel);

        live2DModel.update();
    }


    public void OnRenderObject()
    {
        if (live2DModel == null) load();
        if (live2DModel.getRenderMode() == Live2D.L2D_RENDER_DRAW_MESH_NOW) live2DModel.draw();
    }
}