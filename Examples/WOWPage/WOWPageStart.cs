using System;
using System.Collections.Generic;
using Microsoft.LiveLabs.Html;
using Microsoft.LiveLabs.JavaScript.IL2JS;
using Microsoft.LiveLabs.JavaScript.Interop;
using System.Diagnostics;
using WOWPage.OpenJS;
using Microsoft.LiveLabs.JavaScript;

namespace WOWPage
{
    public class WOWPageStart : Page
    {
        
        Canvas canvas = new Canvas();
        

        // Entry point for JavaScript target (boilerplate)
        [EntryPoint]
        public static void Run()
        {
            new WOWPageStart();
        }

        int threejsScriptsCounter = 3;
        public WOWPageStart()
        {
            Browser.Document.Body.Style.Margin = "0px";
            Browser.Document.Body.Style.OverflowX = "hidden";
            Browser.Document.Body.Style.OverflowY = "hidden";

            //Browser.Document.IncludeScripts(EaselJSLoaded, "easeljs-0.7.1.min.js");

            Browser.Document.IncludeScripts(ThreeJSLoaded, "three.js");
            Browser.Document.IncludeScripts(ThreeJSLoaded, "stats.min.js");
            Browser.Document.IncludeScripts(ThreeJSLoaded, "Detector.js");

            Browser.Window.Resize += Window_Resize;
        }

        void Window_Resize(HtmlEvent theEvent)
        {
            //setting "style" attributes for width/height skews the canvas... 
            //canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            //canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";

            canvas.SetAttribute("Width", Browser.Document.ParentWindow.InnerWidth + "px");
            canvas.SetAttribute("Height", Browser.Document.ParentWindow.InnerHeight + "px");

            DrawCanvas();
        }


        
        private void ThreeJSLoaded()
        {
            threejsScriptsCounter--;
            if (threejsScriptsCounter > 0) return;

            

            //canvas.Id = "canvasScene";
            //canvas.SetAttribute("Width", Browser.Document.ParentWindow.InnerWidth + "px");
            //canvas.SetAttribute("Height", Browser.Document.ParentWindow.InnerHeight + "px");

            ////setting "style" attributes for width/height skews the canvas... 
            ////canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            ////canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";

            //Browser.Document.Body.Add(canvas);



            //===========
            //INIT
            //===========
            init();




            //===========
            //RENDER
            //===========
            animate();
            






            Browser.Window.Alert("loaded threejs");
        }

        Stats stats;
        Scene scene;
        Camera camera;
        WebGLRenderer renderer;

        private void init()
        {
            var divContainer = LibraryManager.ThreeJS.CreateRoot();

            camera = LibraryManager.ThreeJS.Camera(Browser.Document.ParentWindow.InnerWidth, Browser.Document.ParentWindow.InnerHeight);
            camera.position.x = 400;

            scene = LibraryManager.ThreeJS.Scene();

            var ambientLight = LibraryManager.ThreeJS.AmbientLight("0x404040");
            scene.add(ambientLight);

            var directionalLight = LibraryManager.ThreeJS.DirectionalLight("0xffffff");
            directionalLight.position.set(0, 1, 0);
            scene.add(directionalLight);

            LibraryManager.ThreeJS.LoadTexture("UV_Grid_Sm.jpg");
            //var material = map.MeshLambertMaterial();



            //object = new THREE.Mesh( new THREE.BoxGeometry( 100, 100, 100, 4, 4, 4 ), material );
            //object.position.set( -200, 0, 0 );
            //scene.add( object );






            renderer = LibraryManager.ThreeJS.WebGLRenderer(true);
            renderer.setSize(Browser.Document.ParentWindow.InnerWidth, Browser.Document.ParentWindow.InnerHeight);

            stats = LibraryManager.ThreeJS.Stats();
            stats.domElement.Style.Position = "absolute";
            stats.domElement.Style.Top = "0px";
            divContainer.Add(stats.domElement);



            renderer.InitUI(divContainer);



        }


        private void animate()
        {
            Browser.Window.RequestAnimationFrame(animate);

            render();
            if(stats!=null) stats.update();
        }

        private void render()
        {
            var timer = DateTime.Now.Millisecond * 0.0001;

            camera.position.x = (int)(Math.Cos(timer) * 800);
            camera.position.z = (int)(Math.Sin(timer) * 800);

            camera.lookAt(scene.position);



            renderer.render(scene, camera);
        }



        private void EaselJSLoaded()
        {

            canvas = new Canvas();

            canvas.Id = "canvasScene";
            canvas.SetAttribute("Width", Browser.Document.ParentWindow.InnerWidth + "px");
            canvas.SetAttribute("Height", Browser.Document.ParentWindow.InnerHeight + "px");

            //setting "style" attributes for width/height skews the canvas... 
            //canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            //canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";

            Browser.Document.Body.Add(canvas);

            DrawCanvas();


        }

        Stage stage = null;
        Graphics graphics1 = null;
        Graphics graphics2 = null;
        private void DrawCanvas()
        {
            if (stage == null) stage = LibraryManager.CreateJS.Stage("canvasScene");
            if (graphics1 == null) graphics1 = LibraryManager.CreateJS.Graphics();
            if (graphics2 == null) graphics2 = LibraryManager.CreateJS.Graphics();



            //IMAGE
            var bitmap = LibraryManager.CreateJS.Bitmap("KeystoneLoginBackground01.png");
            stage.addChild(bitmap);

            //UPDATE
            stage.update();

            //CIRCLE 1
            graphics1.setStrokeStyle(1);
            graphics1.beginStroke("#800080");
            graphics1.beginFill("#800080");
            graphics1.drawCircle(100, 100, 10);
            var shape = LibraryManager.CreateJS.Shape(graphics1);

            //CIRCLE 1 CLICK
            shape.addEventListener("click", (e) =>
            {

                Browser.Window.Alert("circle 1 click");
            });

            stage.addChild(shape);


            //UPDATE
            stage.update();
            stage.clear();
            //graphics1.clear();


            //CIRCLE 2
            graphics1.setStrokeStyle(1);
            graphics1.beginStroke("#800080");
            graphics1.beginFill("#800080");
            graphics1.drawCircle(500, 400, 40);
            var shape3 = LibraryManager.CreateJS.Shape(graphics1);
            var container1 = LibraryManager.CreateJS.Container();
            container1.addChild(shape3);

            //CIRCLE 2 CLICK
            shape3.addEventListener("click", (e) =>
            {
                Browser.Window.Alert("circle 2 click");
            });


            stage.addChild(container1);




            //UPDATE
            stage.update();


            //RECTANGLE
            graphics2.setStrokeStyle(5);
            graphics2.beginStroke("#800080");
            graphics2.beginFill("#800080");
            graphics2.drawRoundRect(110, 110, 180, 90, 0);
            var shape2 = LibraryManager.CreateJS.Shape(graphics2);
            stage.addChild(shape2);

            //UPDATE
            stage.update();


            //TEXT
            var text = LibraryManager.CreateJS.Text("Hello World", "20px Arial", "#ff7700");
            text.x = 150;
            text.y = 155;
            text.textBaseline = "alphabetic";
            stage.addChild(text);

            //UPDATE
            stage.update();
            


            //TICKER
            LibraryManager.CreateJS.Ticker.addEventListener("tick", (evt)=>{
                text.x += 1;
                Browser.Window.Alert("got here");
                stage.update();
            });


            
            //Browser.Window.Alert("finished createjs stuff");


        }
    }



    public static class LibraryManager
    {

        public static EaselJS CreateJS;


        public static ThreeJS ThreeJS;

    }


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

        public DivContainer CreateRoot(){
            return new DivContainer();
        }

        [Import(@"function(url){ 
            
            var map = THREE.ImageUtils.loadTexture('UV_Grid_Sm.jpg'); 
            //alert(map.wrapS);  //<==== why is the loadTexture not working :( :( :(
            //map.wrapS = map.wrapT = THREE.RepeatWrapping; 
            //map.anisotropy = 16; 
            //return map; 
        }")]
        extern public void LoadTexture(string url);

        [Import(@"function(){ return new Three.BoxGeometry( 100, 100, 100, 4, 4, 4 );  }")]
        extern public BoxGeometry BoxGeometry();


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
    public class Mesh
    {
        [Import(Creation = Creation.Object)]
        extern public Mesh();


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

    }

    [Import]
    public class Scene
    {
        [Import(Creation = Creation.Object)]
        extern public Scene();

        extern public void add(AmbientLight light);
        extern public void add(DirectionalLight light);
        extern public Position position { get; set; }
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


    [Export(PassInstanceAsArgument = true)]
    public delegate void LoopCallback();
}
