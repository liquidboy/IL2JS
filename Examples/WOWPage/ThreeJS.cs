using System;
using System.Collections.Generic;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using Microsoft.LiveLabs.JavaScript.Interop;
using System.Diagnostics;
using WOWPage.OpenJS;

namespace WOWPage
{


    [Import]
    public class ThreeJS
    {
        [Import(@"function(w,h){ return new THREE.PerspectiveCamera( 45, w/h, 1, 2000 ); }")]
        extern public Camera Camera(int width, int height);

        [Import(@"function(){ return new THREE.Scene(); }")]
        extern public Scene Scene();

        [Import(@"function(col){ return new THREE.AmbientLight( col );  }")]
        extern public AmbientLight AmbientLight(string color);

        [Import(@"function(col){ return new THREE.DirectionalLight( col );  }")]
        extern public DirectionalLight DirectionalLight(string color);

        [Import(@"function(aa){ return new THREE.WebGLRenderer( { antialias: aa } );  }")]
        extern public WebGLRenderer WebGLRenderer(bool antialias);

        [Import(@"function(){ return new Stats();  }")]
        extern public Stats Stats();

        [Import(@"THREE.RepeatWrapping")]
        extern public bool RepeatWrapping { get; set; }

        public DivContainer CreateRoot()
        {
            return new DivContainer();
        }

        [Import(@"function(url){ 
            
            return THREE.ImageUtils.loadTexture(url); 
            //alert(map.wrapS);  //<==== why is the loadTexture not working :( :( :(
            //map.wrapS = map.wrapT = THREE.RepeatWrapping; 
            //map.anisotropy = 16; 
            //return map; 
        }")]
        extern public TextureMap LoadTexture(string url);

        [Import(@"function(){ return new Three.BoxGeometry( 100, 100, 100, 4, 4, 4 );  }")]
        extern public BoxGeometry BoxGeometry();

        [Import(@"function(map){ return new THREE.MeshLambertMaterial( { ambient: 0xbbbbbb, map: map, side: THREE.DoubleSide } ); }")]
        extern public Material MeshLambertMaterial(TextureMap map);

        [Import(@"function(geometry, material){ return new THREE.Mesh( geometry, material ); }")]
        extern public Mesh Mesh(SphereGeometry geometry, Material material);

        [Import(@"function(){ return new THREE.AxisHelper( 50 ); }")]
        extern public AxisHelper AxisHelper();

        [Import(@"function(x,y,r){ return new THREE.SphereGeometry( x, y, r ); }")]
        extern public SphereGeometry SphereGeometry(int x, int y, int r);
    }

    [Import]
    public class SphereGeometry
    {
        [Import(Creation = Creation.Object)]
        extern public SphereGeometry();

        extern public Position position { get; set; }
    }

    [Import]
    public class AxisHelper
    {
        [Import(Creation = Creation.Object)]
        extern public AxisHelper();

        extern public Position position { get; set; }
    }


    [Import]
    public class Texture
    {
        [Import(Creation = Creation.Object)]
        extern public Texture();

        //[Import(@"function(inst){ return new THREE.MeshLambertMaterial( { ambient: 0xbbbbbb, map: inst, side: THREE.DoubleSide } ); }", PassInstanceAsArgument = true)]
        //extern public Mesh MeshLambertMaterial();
    }

    [Import]
    public class RepeatWrapping
    {
        [Import(Creation = Creation.Object)]
        extern public RepeatWrapping();

    }

    [Import]
    public class TextureMap
    {
        [Import(Creation = Creation.Object)]
        extern public TextureMap();

        extern public bool wrapS { get; set; }
        extern public bool wrapT { get; set; }
        extern public int anisotropy { get; set; }

        [Import(@"function(inst){ return new THREE.MeshLambertMaterial( { ambient: 0xbbbbbb, map: inst, side: THREE.DoubleSide } ); }", PassInstanceAsArgument = true)]
        extern public Material MeshLambertMaterial();
    }

    [Import]
    public class Material
    {
        [Import(Creation = Creation.Object)]
        extern public Material();

        [Import(@"function(inst){ return new THREE.MeshLambertMaterial( { ambient: 0xbbbbbb, map: inst, side: THREE.DoubleSide } ); }", PassInstanceAsArgument = true)]
        extern public Mesh MeshLambertMaterial();
    }


    [Import]
    public class Mesh
    {
        [Import(Creation = Creation.Object)]
        extern public Mesh();

        extern public Position position { get; set; }

    }

    [Import]
    public class BoxGeometry
    {
        [Import(Creation = Creation.Object)]
        extern public BoxGeometry();

    }


    [Import]
    public class DivContainer : HtmlElement
    {
        public DivContainer(JSContext ctxt) : base(ctxt) { }

        [Import(@"function(){ var container = document.createElement(""div""); document.body.appendChild( container ); return container; }")]
        extern public DivContainer();
    }


    [Import]
    public class Stats
    {
        [Import(Creation = Creation.Object)]
        extern public Stats();

        extern public void update();
        extern public HtmlElement domElement { get; set; }

    }


    [Import]
    public class Camera
    {
        [Import(Creation = Creation.Object)]
        extern public Camera();

        extern public Position position { get; set; }
        extern public void lookAt(Position position);

        extern public double aspect { get; set; }
        extern public void updateProjectionMatrix();
    }

    [Import]
    public class Scene
    {
        [Import(Creation = Creation.Object)]
        extern public Scene();

        extern public void add(AmbientLight light);
        extern public void add(DirectionalLight light);
        extern public void add(Mesh mesh);
        extern public void add(AxisHelper axis);
        extern public void add(SphereGeometry geometry);

        [Import(@"function(inst, index){ return inst.children[ index ]; }", PassInstanceAsArgument = true)]
        extern public SceneItem getChild(int index);

        [Import(@"function(inst){ return inst.children.length;  }", PassInstanceAsArgument = true)]
        extern public int getChildrenLength();

        extern public Position position { get; set; }
    }

    [Import]
    public class SceneItem
    {
        [Import(Creation = Creation.Object)]
        extern public SceneItem();
        extern public Rotation rotation { get; set; }
    }

    [Import]
    public class AmbientLight
    {
        [Import(Creation = Creation.Object)]
        extern public AmbientLight();
    }

    [Import]
    public class DirectionalLight
    {
        [Import(Creation = Creation.Object)]
        extern public DirectionalLight();
        extern public Position position { get; set; }
    }

    [Import]
    public class WebGLRenderer
    {
        [Import(Creation = Creation.Object)]
        extern public WebGLRenderer();

        extern public void setSize(int width, int height);


        //[Import(@"function(inst){ var container = document.createElement( 'div' ); document.body.appendChild( container ); container.appendChild( inst.domElement );  }", PassInstanceAsArgument = true)]
        [Import(@"function(inst, rootDiv){ rootDiv.appendChild( inst.domElement );  }", PassInstanceAsArgument = true)]
        extern public void InitUI(DivContainer div);


        extern public void render(Scene scene, Camera camera);

    }

    [Import]
    public class Position
    {
        [Import(Creation = Creation.Object)]
        extern public Position();

        extern public int x { get; set; }
        extern public int y { get; set; }
        extern public int z { get; set; }
        extern public void set(int x, int y, int z);
    }

    [Import]
    public class Rotation
    {
        [Import(Creation = Creation.Object)]
        extern public Rotation();

        extern public double x { get; set; }
        extern public double y { get; set; }

    }


    [Export(PassInstanceAsArgument = true)]
    public delegate void LoopCallback();
}
