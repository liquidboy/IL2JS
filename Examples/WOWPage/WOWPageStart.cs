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
        Stats stats;

        // Entry point for JavaScript target (boilerplate)
        [EntryPoint]
        public static void Run()
        {
            new WOWPageStart();
        }

        int directCanvasScriptsCounter = 2;
        int threejsScriptsCounter = 6;
        public WOWPageStart()
        {
            Browser.Document.Body.Style.Margin = "0px";
            Browser.Document.Body.Style.OverflowX = "hidden";
            Browser.Document.Body.Style.OverflowY = "hidden";
            Browser.Document.Body.Style.BackgroundColor = "#fff";

            //Browser.Document.IncludeScripts(EaselJSLoaded, "easeljs-0.7.1.min.js");

            //Browser.Document.IncludeScripts(ThreeJSLoaded, "three.js");
            //Browser.Document.IncludeScripts(ThreeJSLoaded, "stats.min.js");
            //Browser.Document.IncludeScripts(ThreeJSLoaded, "Detector.js");
            //Browser.Document.IncludeScripts(ThreeJSLoaded, "CurveExtras.js");
            //Browser.Document.IncludeScripts(ThreeJSLoaded, "ParametricGeometries.js");
            //Browser.Document.IncludeScripts(ThreeJSLoaded, "UVsUtils.js");

            Browser.Document.IncludeScripts(DirectCanvasLoaded, "DirectCanvas.js");
            Browser.Document.IncludeScripts(DirectCanvasLoaded, "stats.min.js");

            Browser.Window.Resize += Window_Resize;
        }

        void Window_Resize(HtmlEvent theEvent)
        {
            ////setting "style" attributes for width/height skews the canvas... 
            ////canvas.Style.Width = Browser.Document.ParentWindow.InnerWidth + "px";
            ////canvas.Style.Height = Browser.Document.ParentWindow.InnerHeight + "px";

            //canvas.SetAttribute("Width", Browser.Document.ParentWindow.InnerWidth + "px");
            //canvas.SetAttribute("Height", Browser.Document.ParentWindow.InnerHeight + "px");

            //camera.aspect = Browser.Document.ParentWindow.InnerWidth / Browser.Document.ParentWindow.InnerHeight;
            //camera.updateProjectionMatrix();

            //renderer.setSize(Browser.Document.ParentWindow.InnerWidth, Browser.Document.ParentWindow.InnerHeight);

            ////DrawCanvas();


            LibraryManager.DirectCanvas.Resize(Browser.Window.InnerWidth, Browser.Window.InnerHeight);

        }


        
        private void DirectCanvasLoaded() {

            directCanvasScriptsCounter--;
            if (directCanvasScriptsCounter > 0) return;

            try
            {

                var div = LibraryManager.DirectCanvas.CreateRoot();

                stats = LibraryManager.ThreeJS.Stats();
                stats.domElement.Style.Position = "absolute";
                stats.domElement.Style.Top = "0px";
                div.Add(stats.domElement);


                DirectCanvasLoop();

            }
            catch (Exception ex)
            {
                Browser.Window.Alert("error .... " + ex.Message);
            }
        }

        private void DirectCanvasLoop()
        {
            Browser.Window.RequestAnimationFrame(DirectCanvasLoop);

            if (stats != null) stats.update();


            DirectCanvasRender();

        }

        private void DirectCanvasRender()
        {
            LibraryManager.DirectCanvas.Context.LineWidth = 2;
            LibraryManager.DirectCanvas.Context.StrokeStyle = "red";

            LibraryManager.DirectCanvas.Context.FillStyle = "#9ea7b8";
            LibraryManager.DirectCanvas.Context.FillRect(0, 0, Browser.Window.InnerWidth, Browser.Window.InnerHeight);
        }

        
        private void ThreeJSLoaded()
        {
            threejsScriptsCounter--;
            if (threejsScriptsCounter > 0) return;

            //Browser.Window.Alert("loaded all threejs scripts ...");
            

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

        
        Scene scene;
        Camera camera;
        WebGLRenderer renderer;

        private void init()
        {
            var divContainer = LibraryManager.ThreeJS.CreateRoot();

            camera = LibraryManager.ThreeJS.Camera(Browser.Document.ParentWindow.InnerWidth, Browser.Document.ParentWindow.InnerHeight);
            camera.position.y = 400;

            scene = LibraryManager.ThreeJS.Scene();

            //var ambientLight = LibraryManager.ThreeJS.AmbientLight("0x404040");
            //scene.add(ambientLight);

            //var directionalLight = LibraryManager.ThreeJS.DirectionalLight("0xffffff");
            //directionalLight.position.set(0, 0, 1);
            //scene.add(directionalLight);

            var map = LibraryManager.ThreeJS.LoadTexture("UV_Grid_Sm.jpg");
            map.wrapS = map.wrapT = true; //LibraryManager.ThreeJS.RepeatWrapping;
            map.anisotropy  = 16;


            var material1 = LibraryManager.ThreeJS.MeshLambertMaterial(map);
            var material2 = LibraryManager.ThreeJS.MeshBasicMaterial();

            var materials = new Material[] { material1, material2 };

            //var material = map.MeshLambertMaterial(); //  LibraryManager.ThreeJS.MeshLambertMaterial(map); 

            //var heightScale = 1;
            //var p = 2;
            //var q = 3;
            //int radius = 150, tube = 10, segmentsR = 50, segmentsT = 20;


            var grannyKnot = LibraryManager.ThreeJS.GrannyKnot();

            var tube2 = grannyKnot.TubeGeometry();  //LibraryManager.ThreeJS.TubeGeometry(grannyKnot);

            var obj7 = LibraryManager.ThreeJS.createMultiMaterialObject(tube2, materials);
            obj7.position.set(100, 0, 0);
            scene.add(obj7);


            ////var obj1 = LibraryManager.ThreeJS.Mesh(material); //material.GetNewMesh();
            //var obj1 = material.MeshLambertMaterial();
            //obj1.position.set( 0, 0, 0 );
            //scene.add(obj1);


            //var obj10 = LibraryManager.ThreeJS.Mesh( LibraryManager.ThreeJS.SphereGeometry(75, 20, 30), material);
            //obj10.position.set( 0, 0, 0 );
            //scene.add( obj10 );


            //object = new THREE.Mesh( new THREE.IcosahedronGeometry( 75, 1 ), material );
            //object.position.set( -200, 0, 200 );
            //scene.add( object );


            //object = new THREE.Mesh( new THREE.OctahedronGeometry( 75, 2 ), material );
            //object.position.set( 0, 0, 200 );
            //scene.add( object );


            //object = new THREE.Mesh( new THREE.TetrahedronGeometry( 75, 0 ), material );
            //object.position.set( 200, 0, 200 );
            //scene.add( object );











            //var obj2 = LibraryManager.ThreeJS.AxisHelper();
            //obj2.position.set(0, 0, 0);
            //scene.add(obj2);



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

            //Browser.Window.Alert(scene.children.length.ToString());
            int len = scene.getChildrenLength();
            for (var i = 0; i < len; i++)
            {
                var obj = scene.getChild(i);

                obj.rotation.x = timer * 5;
                obj.rotation.y = timer * 2.5;
            }


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



   

    
}
