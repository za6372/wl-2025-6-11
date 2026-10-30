using Assets.HotFix.MyUtils.src.data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace Assets.HotFix.xq2d.src.faceI
{
    public interface taskDesInterface
    {
        public void getTaskDes(string key, Action<JObject> ac);
        //public JObject getTaskDes0(string key);
    }
    public class taskDesInterfaceImpl : taskDesInterface
    {
        /**获取任务描述*/
        public void getTaskDes(string key,Action<JObject> ac)
        {
            //读取xml并封装结构
            List<string> all = new List<string>() { "task_talk_xml" };
            DoGet.getInstance().collectionAnyRes(() =>
            {
                XDocument doc = DoGet.getInstance().readXml("task_talk_xml");
                //var element = doc.Element("config").Element("data");
                var ds = doc.Descendants("data").Where(b => b.Attribute("id").Value == key);
                if (ds == null || ds.ToList().Count == 0)
                {
                    JObject taskDes = new JObject();
                    addArr(taskDes, "get",
                            addTaskTalk(1, "少侠，看你一表人才，没想到。。。"));
                    addArr(taskDes, "p0",
                        addTaskTalk(0, "xxx@**......"));
                    addArr(taskDes, "p1",
                        addTaskTalk(0, "1xxx@**......"));
                    addArr(taskDes, "p2",
                        addTaskTalk(0, "2xxx@**......"));
                    addArr(taskDes, "over",
                        addTaskTalk(1, "啊。。。"));
                    ac(taskDes);
                    return;
                }
                var list = ds.ToList()[0].Elements();
                JObject a = new JObject();
                foreach (var p1 in list)//get\p0\p1\over
                {
                    var list2 = p1.Elements();
                    JArray arr = new JArray();
                    foreach (var p2 in list2)//it->{type,talk}
                    {
                        JObject it = new JObject();
                        it["type"] = p2.Element("type").Value;
                        it["talk"] = p2.Element("talk").Value;
                        //Debug.Log(it);
                        arr.Add(it);
                    }
                    a[p1.Name.ToString()] = arr;
                }
                //Debug.Log(a);
                ac(a);
            }, all);
        }
        /*public JObject getTaskDes0(string key)
        {
            JObject taskDes = new JObject();
            switch (key)
            {

                case "1000":
                    {//简单对话即可完成
                        addArr(taskDes, "get",
                            addTaskTalk(0, "你在说什么？可以大点声音吗？"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "少侠你醒了，这里是青云村，我是青云村村长。你在玄都西郊被血盟妖兽袭击，是鹿尧将你送回来的，医师探查过你的身体，没什么大碍。"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "呼，原来是一场梦啊，我是<color=#f9c831><player_name></color>，这次多谢您的帮助村长是否知道是何人所为。"));
                        addArr(taskDes, "p2",
                            addTaskTalk(1, "我听鹿尧说是一只血盟的妖兽动的手，鹿尧现在正在西郊抓它。"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "那正好，我也去帮忙。"));
                        break;
                    }
                case "1001":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "看来你恢复得不错嘛，不枉我照顾了你这几天，我在你身上看到了这样一把钥匙，不知道是有何用。"),
                            addTaskTalk(0, "多谢姑娘这段时间的照顾了，此物是唤灵钥匙，可以收服妖兽做宠物，我展示给你看。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "这次运气不错，居然获得了一只熊猫宝宝，以后你找到这一类的钥匙，也可以试试哦。好了，我要出发去帮鹿尧抓那只血盟妖兽了。"),
                            addTaskTalk(1, "你是说你要去抓血盟的妖兽吗？我也要去。"),
                            addTaskTalk(0, "不行，你去太危险了，你年纪还小，跟过去容易受伤。"),
                            addTaskTalk(1, "不怕，我可以保护自己，若是不信，我们去外面过两招？"));
                        addArr(taskDes, "p1",
                            addTaskTalk(1, "就在这里吧，我已经练武三年了，你小心。"),
                            addTaskTalk(0, "好，姑娘出手吧。"));
                        addArr(taskDes, "p2",
                            addTaskTalk(0, "没想到姑娘身手这般好，你要愿意与我结伴同行，那也正好有个照应。"),
                            addTaskTalk(1, "耶！我终于可以出山了。"),
                            addTaskTalk(0, "（黑线）······"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "我们现在就去找鹿尧吗？"),
                            addTaskTalk(0, "嗯，听说鹿尧正在西郊围捕血盟妖兽，我们现在出发，说不定赶得到帮他。"),
                            addTaskTalk(1, "出发之前，先对刚刚获得的熊嘟嘟进行洗炼吧。洗炼可以使宠物变强，运气好宠物还会发生<color=#f9c831>变异</color>哦！"),
                            addTaskTalk(1, "变异后的宠物，实力更是得到了质的飞跃。这样我们对抗血盟妖兽就更有信心啦。"));
                        break;
                    }
                case "1002":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "果然是修行之人，恢复能力的确了得。我是青云村的医师医仙儿，之前你被送来的时候，就是我帮你治疗的伤势。"),
                            addTaskTalk(0, "多谢姑娘的帮助。"),
                            addTaskTalk(1, "这件事你要多谢鹿尧，是他发现你的。"),
                            addTaskTalk(0, "我们确实是准备去找鹿尧，然后帮他抓住那只血盟妖兽。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "看你周身力量似乎无法控制，可能是要晋升了，看来这次的受伤反而让你因祸得福了。"),
                            addTaskTalk(0, "晋升是什么意思？"),
                            addTaskTalk(1, "你应该即将会进行第一次晋升，晋升之后便可提升自己的属性和强化技能。"));
                        addArr(taskDes, "p1",
                            addTaskTalk(1, "晋升后你的状态看起来好多了，周身看起来充满了力量。但是血盟应该还会再次来犯，你要多加小心。"),
                            addTaskTalk(0, "兵来将挡水来土掩，守住心中的正义，我定能无惧心魔。"),
                            addTaskTalk(1, "经此一劫，得到了晋升的机会，你的技能也可以提升了哦！"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "心魔的影响看起来已无大碍，少侠可继续前往寻找鹿尧了。"),
                            addTaskTalk(0, "感激不尽，希望仙儿姑娘可与我们一起出发。"),
                            addTaskTalk(1, "我也想与你一同冒险，可是……"));
                        break;
                    }
                case "1003":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "少侠就是鹿尧吧？我叫<color=#f9c831><player_name></color>，此前多谢你出手救了我。"),
                            addTaskTalk(1, "不客气，举手之劳而已。还未曾请教你来西郊是有何事？"),
                            addTaskTalk(0, "最近传出许多散仙失踪的消息，我奉命来此探查，应该就是在这中了血盟埋伏。"),
                            addTaskTalk(1, "我与伙伴们围捕血盟妖兽，可惜被它逃脱了。"),
                            addTaskTalk(0, "我准备去玄都复命，鹿仙友是否一起？"),
                            addTaskTalk(1, "那我们先同行一段吧，我也正好准备前往玄都。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "等等，这里似乎有一道朦胧的神韵波动，我们先去查探一番。"),
                            addTaskTalk(0, "神韵波动？确实奇怪。"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "这是……古神的气息？"),
                            addTaskTalk(1, "我只是一缕残念罢了，谢谢你唤醒了我。我生前乃是紫薇仙子，如今这缕残念，维持不了多久，很快就会消散而去了。"),
                            addTaskTalk(0, "你身上发生了什么事？"),
                            addTaskTalk(1, "这事说来话长了……"));
                        addArr(taskDes, "p2",
                            addTaskTalk(0, "既然你我有缘，我便教你如何使用更强的技能。"),
                            addTaskTalk(1, "多谢仙子教导。"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "接下来的路，便只能靠你自己去走了，你说你是在西郊被血盟的妖兽袭击昏迷的，此处必定还有血盟余孽的存在。"),
                            addTaskTalk(1, "就让我用我最后的力量来助你们一臂之力，阴阳眼~开。"),
                            addTaskTalk(1, "仙子这个能力很有用，这让我们对于一些平常不可见之物，有了感应。"));
                        break;
                    }
                case "1004":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "<color=#f9c831><player_name></color>，你在西郊的事情忙完了啊，这是要回去向上神复命吗？"),
                            addTaskTalk(1, "嗯，此行还算有所收获，要尽快返回告知共工上神。"),
                            addTaskTalk(0, "他身上似乎有一些不对劲的地方，有一股类似梦境的力量笼罩着他。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "糟了，这股力量也笼罩到我们了，大家小心......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(1, "你居然能在梦境中战斗，这怎么可能？我当初也被吸入过梦境，却完全无法自动脱离，还是共工上神拯救之后，我才能活着离开。"),
                            addTaskTalk(0, "我也不知道为什么我可以在梦境战斗。"),
                            addTaskTalk(1, "共工上神之前说过，一旦与梦境产生一丝关联，之后便会一直与之接触，看来此次是我连累了你。"),
                            addTaskTalk(0, "何谈什么连累，若不是你在西郊救我，我可能都无法撑到今天。"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "既然你也与梦境产生纠葛，那日后便无法摆脱，我想和你一同面对此事，不知你是否愿意。"),
                            addTaskTalk(0, "欢迎至极。"));
                        break;
                    }
                case "1005":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "传闻南郊有血盟余孽在此生事，少侠且当心啊。"),
                            addTaskTalk(0, "嗯，此事我是知情的，我外出的目的就是探寻血盟踪迹。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "请出示口令。"),
                            addTaskTalk(0, "xxx@**......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "不错，口令正确，欢迎回到南郊。"),
                            addTaskTalk(0, "在此地守卫，辛苦了。"));
                        break;
                    }
                
                //50副本
                case "3163":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "你是来帮忙镇压紫阴古城的？太好了，紫阴古城涌出的妖物越来越多，我们这边忍受严重缺着。"),
                            addTaskTalk(0, "现在紫阴古城内是什么情况？"),
                            addTaskTalk(1, "我也不是很清楚，但是有专门的调查人员已进入紫阴废墟，请你进去后在古城外围猎杀噬人妖，将那些妖核交给他研究。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "这是你取来的妖核？很好！我正需要它们。年轻人，你帮了我大忙。"));
                        break;
                    }
                case "3164":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "你查到了什么？"),
                            addTaskTalk(1, "有些眉目了，但还不是很确定，我现在要借助你的妖核打开这里的一个封印，看看什么情况。"),
                            addTaskTalk(1, "古城调查者将妖核放入自己布置的阵中，妖核消融蒸发，不远处一阵红光刺目，传来尖锐的笑声。噬魂魔将：哪个不知死活的家伙打扰我休息？"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "是噬魂魔将？！"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "想不到，仅仅在这里就出现了噬魂魔将，看来紫阴古城潜伏的妖魔比想象钟还要可怕。"));
                        break;
                    }
                case "3165":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "这么说来，紫阴古城的威胁性比原本预估的还要高？"),
                            addTaskTalk(1, "是要高很多！糟了，镇魔将军孤身潜入了紫阴废墟，不知道会不会有危险，请你速速去告诉他紫阴古城的危险性！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "你是镇魔将军？"),
                            addTaskTalk(1, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "你没事吧？"),
                            addTaskTalk(1, "......"));
                        break;
                    }
                case "3166":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "......"),
                            addTaskTalk(1, "......快......走......"),
                            addTaskTalk(0, "你说话了？你说什么？"),
                            addTaskTalk(1, "魔......帅......！"),
                            addTaskTalk(1, "突然一阵阴笑凭空升起，一股浓郁的黑气自地面浮起，化作狰狞的面孔。魔帅：又来一个，正好给我进补！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "好险！你怎么样？"),
                            addTaskTalk(1, "你......居然能击败魔帅！"),
                            addTaskTalk(0, "这是什么情况？"),
                            addTaskTalk(1, "说来惭愧，我本来想进来一探究竟，没想到被那魔帅偷袭，遭到他的控制，囚于此地。"));
                        break;
                    }
                case "3167":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "你可有调查到什么？"),
                            addTaskTalk(1, "那魔帅逮到我后得意忘形，透露了很多事情。紫阴古城深处的 紫阴中枢中存在着太古真魔……"),
                            addTaskTalk(0, "太古真魔？好霸道的名字！"),
                            addTaskTalk(1, "他可不是徒有虚名之辈，你最好先击溃暴怒尸鬼，再与他决战！"),
                            addTaskTalk(0, "为何？"),
                            addTaskTalk(0, "因为暴怒尸鬼会增强太古真魔的实力！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "人类吗？竟敢深入紫阴古城，不知死活！"),
                            addTaskTalk(0, "你们也想入侵尘世吗？"),
                            addTaskTalk(1, "你没有资格知道！"));
                        break;
                    }
                case "3168":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "那就把你的魔核交出来吧。"),
                            addTaskTalk(1, "区区人类，竟然敢在我面前大言不惭！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "就是这个魔核吧？"),
                            addTaskTalk(1, "没错，就是它，没想到你真的能拿到了！"),
                            addTaskTalk(0, "（感情你就没想过我能拿到啊）这东西能做什么？"),
                            addTaskTalk(1, "太古真魔的魔气之源，只要有它，我们就可以根据魔气的属性修补上古封印，将紫阴古城重新镇压回空间裂缝中。"));
                        break;
                    }
                //60
                case "3169":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "你就是接受悬赏榜文来探查万魂沟壑的人？"),
                            addTaskTalk(0, "是的！"),
                            addTaskTalk(1, "年纪轻轻，胆量却是不小，万魂沟壑如今煞气越来越重，不时有怨魂涌出，必须尽快处理。"),
                            addTaskTalk(0, "要怎么做？"),
                            addTaskTalk(1, "我们从怨魂处得知沟壑中的亡灵裨将似乎知道些什么，你得去落矢绝地中寻找它，在此之前，为了避免被群起而攻，我想你应该清理掉一些葬刀绝地的亡灵刀兵。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你来了？"));
                        break;
                    }
                case "3170":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "万魂沟壑的煞气越来越强烈，是你们搞的鬼？"),
                            addTaskTalk(1, "......"),
                            addTaskTalk(0, "你们有什么阴谋？"),
                            addTaskTalk(1, "......"),
                            addTaskTalk(0, "......"),
                             addTaskTalk(1, "问够了？那就受死吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "又要被人所杀了吗......"),
                            addTaskTalk(1, "那是因为你要杀人！"),
                            addTaskTalk(1, "呵......可笑，我们就是因为被人所杀才于此不停的游荡厮杀，你知道我们这些怨灵的痛苦吗？"));
                        break;
                    }
                case "3171":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "......"),
                            addTaskTalk(1, "你不会懂的，当我们在绝望之中被杀戮，当我们在这个暗无天日的沟壑里相互厮杀的时候，这一切就已经注定！"),
                            addTaskTalk(0, "扫清浊世？你们......"),
                            addTaskTalk(1, "不要以为你赢了！我麾下还有亡灵戟兵！"),
                            addTaskTalk(0, "嗯？好多士兵，似乎被包围了啊？真实麻烦，先解决这些家伙吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "可恶......"));
                        break;
                    }
                case "3172":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "结束了，放下吧！"),
                            addTaskTalk(1, "放下？哈哈哈......你们这些人只会说这些风凉话！"),
                            addTaskTalk(0, "......"),
                            addTaskTalk(1, "别以为你真的赢了，亡灵统帅大人必将率领亡灵斧兵和万千怨魂扫平尘世！"),
                            addTaskTalk(0, "亡灵统帅......亡灵斧兵吗？看来又是个麻烦，先将这些亡灵斧兵解决吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "生者，来我们怨魂聚集之地是挑衅吗？"),
                            addTaskTalk(0, "收手吧，滥造杀孽，只会让你被束缚，不得入轮回。"),
                            addTaskTalk(1, "收手？当年彭城之战，我们被逼往潍水残杀的时候，楚军可曾收手？我们要报仇！为了死去的我们，为了因我们而死的人，我们必须报仇！"));
                        break;
                    }
                case "3173":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "报仇？你所滥杀的那些是你的仇人？你要报的是谁的仇？谁才是你的仇人？"),
                            addTaskTalk(1, "是......是......"),
                            addTaskTalk(0, "答不出来吧？正所谓冤冤相报何时了，人间已经够乱了，你又何必……"),
                            addTaskTalk(1, "不！我要报仇！该死的生者，想动摇我的意志吗？受死成为我的部卒吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "我居然数量！可恶，可恶啊！"),
                            addTaskTalk(0, "结束了，放弃你那无谓的计划吧！"),
                            addTaskTalk(1, "我错了吗？我错了吗......"));
                        break;
                    }
                case "3174":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "你没有错，错的是人！"),
                            addTaskTalk(1, "人？是人吗？哈哈哈……人啊！对啊，曾几何时我也曾是人，也曾亲手杀死无数不相识的人……可悲的人……"),
                            addTaskTalk(0, "......"),
                            addTaskTalk(1, "我明白了，这是 我的魔煞帅印，我以无数冤 魂凝聚而成的东西。交给你 吧，用它，将我们镇压在这 里，直到能入往生！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你居然还能回来！"),
                            addTaskTalk(0, "......这是魔煞帅印，用它可以镇压万魂沟壑的怨魂，交给你们了！"),
                            addTaskTalk(1, "真的？居然能拿到这种奇物！那我便代天下苍生感谢你了！"));
                        break;
                    }
                //70
                case "3175":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "年轻人，你是来助我的？"),
                            addTaskTalk(0, "......算是吧！这里发生了什么事？"),
                            addTaskTalk(1, "自从七星和紫薇帝星降临之后，太阴之月的阴气愈发衰弱，导致世间阴阳失衡，久而久之必将对凡人产生深远的不良影响……"),
                            addTaskTalk(0, "阴阳失衡？这么严重！"),
                            addTaskTalk(1, "是的，这隐月幽谷有太阴之精，能中和阴阳。但想进去，必须击败猛兽之林的守谷老人……你愿意帮忙吧？那便进去吧，对了，在对付守谷老人之前，最好先消灭巨山熊，免得守谷老人调动，让你陷入困境！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "外人，可有隐月幽谷的月谷令？"),
                            addTaskTalk(0, "月谷令？没有......"),
                            addTaskTalk(1, "那么你不应该进去。"));
                        break;
                    }
                case "3176":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "为什么？"),
                            addTaskTalk(1, "没有月谷令，进去只会被太阴之力侵体，阴阳失衡而亡。"),
                            addTaskTalk(0, "月谷令从哪里来？"),
                            addTaskTalk(0, "由隐月宗赐予，或者......击败我！"),
                            addTaskTalk(1, "隐月宗？闻所未闻，看来我只能得罪了！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "......不错，你的实力，有资格获得这月谷令！"));
                        break;
                    }
                case "3177":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "谢了，你知不知道太阴 之精在哪？"),
                            addTaskTalk(1, "太阴之精？那东 西掌握在隐月宗宗主月尘子 手中。"),
                            addTaskTalk(0, "月尘子吗？好，我去见他一见。"),
                            addTaskTalk(0, "我劝你不要直接去见他，因为月尘子不喜外人，若想见他最好能得他的弟子通报，你入谷之后，可先去清风幽径找他的二弟子。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哦？居然是 个外来之人，好久未曾见过 外来人了！"),
                            addTaskTalk(0, "……不知我能不能见你 们宗主？"),
                            addTaskTalk(1, "想见师傅？呵呵，可以给你这个机会。"));
                        break;
                    }
                case "3178":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "真的？"),
                            addTaskTalk(1, "当然，不过前提是......你必须先胜了我！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "啊......大师兄，他欺负我......"));
                        break;
                    }
                case "3179":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "现在可以为我引荐你们宗主了吧？"),
                            addTaskTalk(1, "其实......引荐师傅只有大师兄有这个资格。"),
                            addTaskTalk(0, "什么？你骗我？"),
                            addTaskTalk(1, "那倒也不算，至少我可以让你去兽王古道见大师兄......"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "外来人？你来隐月幽谷为何？"),
                            addTaskTalk(0, "想见你们宗主......"),
                            addTaskTalk(1, "见宗主？倒也不是不可以......"),
                            addTaskTalk(0, "哦？那请为我引荐吧！"),
                            addTaskTalk(1, "呵......是这样的，我与师妹最近正修炼着合体之术......"));
                        break;
                    }
                case "3180":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "合体？！（大弟子头顶突然出现一个娇柔的倩影）"),
                            addTaskTalk(1, "师兄！你胡说什么？ 想死吗！？"),
                            addTaskTalk(1, "咳咳，是合击之术！这样吧，你若能胜我们联手的合击，我便为你引见师傅。倩倩：我什么时候说要跟你联手了？！"),
                            addTaskTalk(1, "师妹，外敌面前，给点面子嘛。师妹：哼！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "想不到你竟能胜过我们的合体……合击之术，看来我们还不够纯熟。师妹，你看我们是不是应该抽出更多时间……倩倩：明明就是你太弱！哼，我走了！"),
                            addTaskTalk(1, "哎，那咱们迟些再讨论加长训练时间的问题呐。"));
                        break;
                    }
                case "3181":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "呵呵......"),
                            addTaskTalk(0, "......你的口水快到地上了。"),
                            addTaskTalk(1, "（神色一正）恩人，不，外来人，这是月尘令，拿着他你就可以去 太阴奇境 见师傅了。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "是谁？打扰我清修。"),
                            addTaskTalk(0, "（拿出月尘令）你知道天地阴阳失衡的事吗？"),
                            addTaskTalk(1, "阴阳失衡？我在谷中修炼已近百年，外界一切与我无关。"),
                            addTaskTalk(0, "……那你可否给我一些太阴之精？"),
                            addTaskTalk(1, "太阴之精？哼，那是我修炼必须之物！"));
                        break;
                    }
                case "3182":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "……相较于阴阳平衡， 众生命数，你的修炼更重要 吗？"),
                            addTaskTalk(1, "嗯？你想晓我以大义？哈哈哈……我月尘子修炼多少岁月，世间种种有何不知？也罢，念在你一路来此，若能战胜我这月尘化身，我便送你太阴之精。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "嗯，看你年纪轻轻，已有这般修为，难怪我那几个不争气的弟子会输给你！"));
                        break;
                    }
                case "3183":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "那现在……是不是可以……"),
                            addTaskTalk(1, "你放心，我月尘子说到做到，这便是太阴之精，你拿去吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你回来了，怎么 样？可有取得太阴之精？"),
                            addTaskTalk(0, "将太阴之精递过）就 是这东西吧？"),
                            addTaskTalk(1, "（激动万分）没错，就是这东西，如此一来，阴阳失衡的情况总算可以缓解了！"));
                        break;
                    }
                //80
                case "3184":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "生者吗？"),
                            addTaskTalk(0, "石碑居然说话......"),
                            addTaskTalk(1, "吾乃往世碑，历无尽岁月而通灵。你既有缘来此，是否有不解之遗憾？"),
                            addTaskTalk(0, "遗憾吗……大概每个人都会有吧。"),
                            addTaskTalk(1, "既如此，便入往生殿，在 前生古道 击败 前世之灵，再去 来生古道 拜访 三生兽吧。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然有生者来到了 这里。生者，未死已有遗憾 了吗？"),
                            addTaskTalk(0, "呃……其实我是被往世 碑送进来的……"),
                            addTaskTalk(1, "往世碑？看来它窥探到了什么......"));
                        break;
                    }
                case "3185":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "窥探到了什么？"),
                            addTaskTalk(1, "这就要问你自己了......"),
                            addTaskTalk(0, "啊？这……"),
                            addTaskTalk(1, "生者，你本不该入 往生殿，但既然已至，我只 能依殿规处理。去吧，先去 击败 今生古道 的 今生之魂 ， 再回来见我！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你完成了？不错.......你还保持着清醒！"));
                        break;
                    }
                case "3186":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "清醒？"),
                            addTaskTalk(1, "来吧，若能战胜我，你便可入内一窥三生轮回碑，若不能，便离去吧。"),
                            addTaskTalk(0, "啊？这……"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "有趣，很久没人能胜过我的一分力了。"),
                             addTaskTalk(0, "一分力......"),
                             addTaskTalk(1, "你的实力尚且微弱，不足以让我使出全力，我本以为一分力已足以让你溃败，没想到你的应变远在常人之上。"));
                        break;
                    }
                case "3187":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "听了你的话，我不知该伤心好还是高兴好……"),
                            addTaskTalk(1, "罢了，你既然胜了，我便让你去 轮回古道 一窥三生轮回碑 吧。"),
                            addTaskTalk(0, "真的？"),
                            addTaskTalk(1, "自然是真的！不过在去之前，先接受来世之魂的考验吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "生者？入往生殿是为了未来还是过去？"),
                             addTaskTalk(0, "为了......现在吧。"),
                             addTaskTalk(1, "嗯，呵呵呵......很好的回答。"));
                        break;
                    }
                case "3188":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "是吗？"),
                            addTaskTalk(1, "是的，世人总喜欢执着于过往，害怕于未来，而不知珍惜现在。"),
                            addTaskTalk(0, "......"),
                            addTaskTalk(1, "生者，你的回答我很满意，但是，生者毕竟是生者，必须依照往生殿千古戒条，给予考验。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "生者，你胜了。"),
                             addTaskTalk(0, "呼......有惊无险的胜了。"),
                             addTaskTalk(1, "前世因，今世果。今世孽，来生报。生生死死，轮回往复，冥冥中有所注定，切记!切记！"),
                             addTaskTalk(0, "冥冥中有注定吗？那人岂非命运的玩物？"),
                             addTaskTalk(1, "不，冥冥天数 ，并非不可违，只是你今生 所为，来世必将有报，宇宙 无穷，生死循环间因果乃是 必然。我能告诉你的就是这 些，你，可以去了。"));
                        break;
                    }
                //90
                case "3189":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "年轻人，我想你帮我一个忙。"),
                            addTaskTalk(0, "什么忙？"),
                            addTaskTalk(1, "青丘境内的四象之灵被盗取了，为恐青丘境内的异兽出来为祸人间需尽快找回四象之灵将青丘境重新封印。用四象之灵封印九尾异兽，使其沉睡才能封印青丘境。我要在此入口守卫，你可否帮我找回四象之灵？青丘入口 的青丘之灵能与四象之灵感应，只要你沾染足够多的青丘之灵灵气，即可指引你找到偷盗之人！"),
                            addTaskTalk(0, "我明白了，交给我吧。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "（幽蓝色的气息渐渐聚集成一股灵气，环绕着眼前的男子）"),
                             addTaskTalk(0, "（是他吗？）"),
                             addTaskTalk(1, "救命啊！"),
                             addTaskTalk(0, "（疑问）怎么回事？"),
                             addTaskTalk(1, "这里很多怪物，我 本想用我的传家宝剑将怪物 除掉，可宝剑却被盗了！"),
                             addTaskTalk(0, "（难道是那盗取四象之 灵的盗贼？）那小偷跑哪了 ？长什么样？"),
                             addTaskTalk(1, "（他横眉大目，相貌凶残，说要借剑给我看，却把剑抢去！"));
                        break;
                    }
                case "3190":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "(盗取四象之灵的盗贼会公然抢劫？)那盗贼可糟了，他还偷了四象之灵，青丘内妖兽都想抢四象之灵，他在里面肯定成白骨了，你放心，等会儿进去就能看见你的宝剑了。"),
                            addTaskTalk(1, "瑞南羽抖了一抖"),
                            addTaskTalk(0, "你就是盗取四象之灵的盗贼吧！"),
                            addTaskTalk(1, "嗯？想不到我竟着 了你的道！倒是有点聪明， 不知道武功怎样？！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "把四象之灵交回来！"),
                             addTaskTalk(1, "放心，既然输了，我就只能乖乖交出来！"),
                             addTaskTalk(0, "为什么要偷四象之灵？"),
                             addTaskTalk(1, "我瑞南羽盗过皇陵 ，偷过皇宫，看遍天下奇物 ，唯独没能一窥这等神物！ 嘿，今天总算见到了，果然 不凡。"));
                        break;
                    }
                case "3191":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "你可知四象之灵移位， 青丘解除封印，里面异兽便 能逃出青丘，会给天下带来 祸害的。"),
                            addTaskTalk(1, "嘿嘿，我知道啊， 可就是技痒！这样吧，让我 将功补过，我告诉你如何引 出九尾异兽吧？"),
                            addTaskTalk(0, "你知道怎么引出来？"),
                            addTaskTalk(1, "我可是盗过不少奇书的！不过嘛，青丘之灵灵气一直环绕着我，如果你不把这灵气消除，我可随时会被怪物袭击啊！你先吓退这附近的青丘之灵吧。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "我可以安心逃跑了……不，我可以安心慢慢告诉你怎么引九尾异兽出来了！嘿嘿！"));
                        break;
                    }
                case "3192":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "奇书曾云，九尾异兽极易暴怒，一暴怒即会仰天长怒吼！要引出九尾异兽很简单，只要杀死数只 禁忌古道 的九尾幻影　，惹怒九尾异兽，它自会出现！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "刚刚我听见一声怒吼，应该就是九尾异兽了！"));
                        break;
                    }
                case "3193":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "听声音大致在迷影禁地附近，就靠你去封印了。我先走了。"),
                            addTaskTalk(0, "你还真是不负责任......"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "那就是九尾异兽？"));
                        break;
                    }
                case "3194":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "（这怪物真大）"),
                            addTaskTalk(1, "九尾异兽怒目圆瞪，看着你手中的四象之灵。"),
                            addTaskTalk(1, "四象之灵？人类，你想封印我？"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "呃......你......(九尾异兽受伤倒地)"));
                        break;
                    }
                case "3195":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "趁这机会，封印了青丘境吧。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "四象之灵发出炫目奇光，包围住九尾异兽，九尾异兽倒下陷入沉睡。\n青丘境被封印"));
                        break;
                    }
                //100
                case "3196":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "年轻人，你是来闯混沌邪灵渊的？"),
                            addTaskTalk(0, "据说有真仙陨落其中，不知是否属实？"),
                             addTaskTalk(1, "非真仙，是一侠客多年前曾有侠客坠入混沌邪灵渊，其身带宝物亦落入洞渊。年轻人，如你替我寻得那侠客，我便恳求他将那宝物送与你。如获宝物必有助于你的修为。"),
                             addTaskTalk(0, "真的？可那敌方听起来很危险......"),
                             addTaskTalk(1, "以你目前的修为，可以一试。入混沌邪灵渊最好便是先从恶灵那里获取灵精石，可以掩盖你身上的人息，避开 裂影渊 中 洞渊战魂的审查。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "（这就是洞渊战魂？）我想进入洞渊深处。"),
                            addTaskTalk(1, "（洞渊战魂上下打量着你）新来的小鬼？进去吧。"));
                        break;
                    }
                case "3197":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "（没想到那么顺利）哈哈，那我进去了。你一个华丽地转身，几个灵石被甩出去了。"),
                            addTaskTalk(1, "人类的气息？"),
                             addTaskTalk(0, "（糟了）......"),
                             addTaskTalk(1, "没想到区区一个 人类，竟能拿到灵精石，有 趣有趣。人类，来一战吧！赢了我便放你进入，输了就 当我的手下，一辈子留在这 里！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "我竟然输了......人类，看你其貌不扬，没想到这么厉害。"),
                            addTaskTalk(0, "什么其貌不扬，我只是帅得不明显！"));
                        break;
                    }
                case "3198":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "人类，你要进洞渊深处做什么？"),
                            addTaskTalk(0, "我为什么要告诉你。"),
                             addTaskTalk(1, "因为我可以帮你。"),
                             addTaskTalk(0, "（你犹豫了一下，把事情告诉他。）"),
                              addTaskTalk(1, "哈哈，原来如此，我知道在哪里。"),
                              addTaskTalk(0, "你知道？"),
                              addTaskTalk(1, "去找6个影魂石， 到陨仙渊交给百鬼之王，他 最喜欢收集影魂石，给他他 便会告诉你的！"),
                              addTaskTalk(0, "（那你就不能告诉我么？）"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "这就是影魂石？小鬼，你有什么要问我的？"),
                            addTaskTalk(0, "你是百鬼之王？"));
                        break;
                    }
                case "3199":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "我想找一位侠客。"),
                            addTaskTalk(1, "哦？侠客？是修道者找你来的？"),
                             addTaskTalk(0, "你知道？那就告诉我他在哪里？"),
                             addTaskTalk(1, "我像那么助人为乐的鬼么？杀死一些魔影证明你的实力再说，我可不跟废物说话！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哈哈，不错不错。"));
                        break;
                    }
                case "3200":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "现在可以告诉我了吧。"),
                            addTaskTalk(1, "赢了我便把一切告诉你！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哈哈，如我所料，根骨绝佳，假以时日，必能成大器。"));
                        break;
                    }
                case "3201":
                    {
                        addArr(taskDes, "get",
                             addTaskTalk(1, "那么我就把一切告诉你吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "多年前我受伤坠入此地，被邪气侵蚀，多亏仙物护体才能保住心脉。如今这百鬼之王倒是做得舒坦。"));
                        break;
                    }
                case "3202":
                    {
                        addArr(taskDes, "get",
                             addTaskTalk(0, "你竟然就是那侠客。"),
                              addTaskTalk(1, "去吧，告诉那修道者，别再来找我了。"),
                              addTaskTalk(0, "你再考虑一下吧，你不回去他会很伤心的，我也会拿不到那宝物的......"),
                              addTaskTalk(1, "那宝物护我心脉后早已化作灰烬。"),
                               addTaskTalk(0, "（白走一趟）......"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "如何？找到那侠客没？"),
                            addTaskTalk(0, "找到是找到了，只是他并不愿意回来。他说日子过得很舒坦，让你不要再找他了。"),
                            addTaskTalk(1, "怎会这样！当年遇 魔，得他相救，却害得他落 入魔境，只可惜我修为不够 ，迟迟未能相救！如今却已 晚了。罢了，各人有各人的 造化。年轻人，你帮了我一个大忙，我渡给你一些仙气，助你提高修为。"),
                            addTaskTalk(0, "你感觉到一股清风袭来，全身舒坦。"));
                        break;
                    }
                //精英1
                case "3203":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "少侠，实在是迫不得已，衙门只好请你来帮忙。近十多年，各地不断有孩童失踪，经过这些年的调查，发现是一个杀手组织所为。我们得到线报，来此处调查，急需你的援手。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "呜呜呜......我好怕......"));
                        break;
                    }
                case "3204":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "其他人呢？"),
                              addTaskTalk(1, "好多妖怪，好可怕......（拼命摇头）"),
                              addTaskTalk(0, "（看来得先把周围的妖怪除去。）"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "......"));
                        break;
                    }
                case "3205":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "那里（手指着左边 地图）！他们都被关在那里 ……那些黑衣人以为我死了 ，把我扔下地窟，我爬着死 人的尸体才来到这里……"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "好美味的香气！哈哈……救了她果然没错，又给我送来了美味的人类！"));
                        break;
                    }
                case "3206":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "我肚子饿了，让我尝尝你的味道吧？"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "（虐杀之鬼身体渐渐消失......）"));
                        break;
                    }
                case "3207":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(0, "那个女孩......欺骗了我？回去找她！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "（小女孩倒在地上。）对不起……对不起（哭泣）我在死人尸堆里，它说只要我替他骗99个人类给他吃，他就让我活下来……我不能死……弟弟还在等着我......"));
                        break;
                    }
                case "3208":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(0, "你的伤……是谁做的？"),
                              addTaskTalk(1, "一个 黑衣服的大哥 哥，他知道我骗他……呵呵……好想再见弟弟……（小女孩身体渐渐冰冷。）"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哼！妇人之仁！想让我死的人，我为何不能杀她？而且，应当是我找你算账吧？"),
                            addTaskTalk(0, "什么意思？"),
                            addTaskTalk(1, "舍弟承蒙你照顾了 ，他命丧你剑下，等我把毁 了我兄弟俩一生的组织完结 后，我也会替他完结你！"));
                        break;
                    }
                case "3209":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(0, "你弟弟是谁？"),
                              addTaskTalk(1, "他是见影！当年组织最后的试验是让我们自相残杀，胜利者才有资格活下来！他舍命保我，半死之体被扔下地窟喂虐杀之鬼，却被天星子救回！他一生坚贞 护主，终究却还是比我这杀 手死在前头！"),
                              addTaskTalk(1, "你进来是要救那些 孩子吧？他们在地牢前，你 最好先把 附近的魔兽消灭掉。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "有人来救我们了！ 太好了！！姐姐在乱葬废墟 呢，她说很快会一起逃出的 ！果然是真的呢！"));
                        break;
                    }
                case "3210":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(0, "……你姐姐……"),
                              addTaskTalk(1, "姐姐被扔下地窟， 我以为她死了，哭了一天， 没想到她还活了下来，偷偷 躲在附近呢！姐姐真厉害！英雄大人，那 主使人 在赤炼血池，求你帮我们消灭她吧！"),
                              addTaskTalk(1, "你进来是要救那些 孩子吧？他们在地牢前，你 最好先把 附近的魔兽消灭掉。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哦？又来了一个 送死的傻蛋？（手指黑衣人 ）青锋，我培养了你十多年 ，你倒是做起内奸了，把我 们的组织大白天下？"));
                        break;
                    }
                case "3211":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "那位仙人说 了，双月重天即将到来，天 命将踏上正轨！你是唯一的 变数，我不得不把你的尸体 留下来！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "我完成不了“那位仙人”交代的事，呵呵......（指着黑衣人）青锋，你弟弟真笨，真笨，他为主牺牲，却不知道一切都是个局！"));
                        break;
                    }
                case "3212":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "我在壶中几百年，天星子把我放出来，让我统领这个组织。见影是他从一开始就看中的孩子，我不过是陪他演了场戏。现在戏谢幕了，就让一切化为冻石吧！（整个洞窟渐渐化为石头， 所有的人都被石化，而你却 什么事也没有。暗影杀手消 失的身躯化为一块闪光的奇 石。）"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你出来了？孩子们呢？"),
                            addTaskTalk(0, "我……如果不是我……他们不会都化成石头……是我……"),
                            addTaskTalk(1, "凝思一会）未必没有解决的办法，不倦先生博古通今，也许会知道点什么。"));
                        break;
                    }
                    //精英2
                case "3213":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "数百年前，七星轮转，妖星现世，苍天被划破，出现一条裂痕，浮游城便是裂痕中异空间之城。浮游城为幻象之城，入内可见过去之景未来之象。"),
                              addTaskTalk(0, "那我该如今找到浮游城 ？"),
                              addTaskTalk(1, "过去未来皆在人心 中。（在你的额头上指划）进去吧 ，孩子。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你是外来的人？外 面的人原来长得和我们一样 的呢！你也喜欢花吗？也喜 欢大树吗？也喜欢风和云吗 ？"),
                            addTaskTalk(0, "是啊，都喜欢。漂亮的妹妹，你知道九黎壶在哪里吗？"));
                        break;
                    }
                case "3214":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "（眨眨眼睛）娘亲 说不能告诉别人哦！但是如 果你陪我玩，本姑娘可以告 诉你在哪里！"),
                              addTaskTalk(0, "好……你想玩什么？"),
                              addTaskTalk(1, "村里的树林里好多好多巨角魔牛哦，你帮我拔几只 牛角 给 我娘亲 吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "......外来之人？"));
                        break;
                    }
                case "3215":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "（露出凶狠的神色）你也是找九黎壶？去 回忆之 地击杀 震岳巨熊 ，我女儿会 告诉你的！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "我娘亲是骗你进来的~她最讨厌外面的人了！"));
                        break;
                    }
                case "3216":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "（远处传来兵马踏过之声。）红衣女：什么声音？好像往 家的方向过去了！"),
                              addTaskTalk(0, "我去看看！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哦？还有死剩的？"));
                        break;
                    }
                case "3217":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "（村庄被焚烧殆尽，四周遍地尸体。）"),
                              addTaskTalk(0, "你是什么人？为什么要残杀老弱妇孺？！"),
                              addTaskTalk(1, "死人是不需要知道任何事情的！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "我竟然败了……呵哈哈哈！（仰天长笑）败又如何，我已经完成“那位仙人”的任务，九黎壶已经拿到手，送出村庄了，死又如何？"));
                        break;
                    }
                case "3218":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "（灭城将军断气了……）你 :那个 女孩 不知道有没有事 ……"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "全死了？娘亲……爹……弟弟……"),
                            addTaskTalk(0, "那是朝廷的人，为什么……"));
                        break;
                    }
                case "3219":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "（擦擦眼泪）我们世代守护九黎壶，只想安稳避世，奈何世人皆想夺去！我好恨……好恨自己……秦朝杀我家人，毁我家园，可我没能力报仇……"),
                               addTaskTalk(0, "他们或许会斩草除根， 先找个地方藏起来吧？"),
                                addTaskTalk(1, "藏起来？要藏起来 忘记大仇？你说，这弑亲烧 庄的大仇该报不该报？"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "你的恨太重，最爱你的 家人是决意不想你冒险报仇 的！"),
                            addTaskTalk(1, "是啊……他们是最爱 我的家人，世上唯一爱的家 人……他们竟将我最爱的人杀去！（眼神坚决）我戚薇发誓，此生必定要将秦朝覆灭！"));
                        break;
                    }
                case "3220":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "（眼前景象快速崩塌，一切 似乎回到虚空。）"),
                               addTaskTalk(0, "回去找玄机子吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "迟了一步，九黎壶 已被夺去？罢了，此乃天命 ，不倦先生应当还知道其他 解决方法。"));
                        break;
                    }
                    //精英3
                case "3221":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "你真的要进去吗 ？先斩杀 黑煞木妖 取其 木之 灵气 护眼可以防止烈火灼眼。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你想听个故事吗？"),
                            addTaskTalk(0, "不想……我赶时间……"),
                            addTaskTalk(1, "话说啊，这隐龙古城曾经是最繁荣美丽的城市，而治理古城的君皇更是天下无双的贤良君皇……"));
                        break;
                    }
                case "3222":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "其实……我真的赶时间……"),
                              addTaskTalk(1, "一日那君皇路经渤海之巅，竟看见一只龙被妖魔袭击，满身是血。君皇只懂皮毛仙术，幸好祖上传下九黎壶，妖魔不敢近身......"),
                              addTaskTalk(0, "九黎壶？"), 
                              addTaskTalk(1, "......咳咳，好口渴......"),
                              addTaskTalk(0, "小的立刻给您取水来！") );
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "你难道就是通天眼？"),
                            addTaskTalk(1, "话说那龙痊愈后竟 化身为妙龄女子，欲嫁与君 皇，原是早已心生爱意，无 奈神女有心襄王无梦，君皇 将龙女送回渤海之巅。龙女思君心切，又受妖魔蛊惑，竟回古城偷去君皇的法力来源九黎壶，并掳走君皇。邻国得知国已无君，朝政一片混乱，遂派大兵压境，屠杀古城子民，占领古城……"));
                        break;
                    }
                case "3223":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "君皇得知后心痛不已，愧对古城百姓，便从城墙跳下去自尽而亡。龙女悲痛悔恨，龙啸九天，古城顷刻化为废墟。她年年日日在此处思念君皇，竟将君皇魂魄锁入壶中，年年月月，君皇已具形态，化身为魔......"),
                              addTaskTalk(0, "那九黎壶现今在 龙女 手上？"),
                              addTaskTalk(1, "笑而不语。）我在龙啸古地等你。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "什么人？给我滚开！不要接近我们！"));
                        break;
                    }
                case "3224":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "滚开！他要和我在一起，你们别想拆散我们……接近我们，都要死！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "（君皇形体化为灰烬……）夫君……夫君……你去哪里？"));
                        break;
                    }
                case "3225":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "（面如死灰）为什么 ？"),
                            addTaskTalk(0, "他早就已经死了，为了 自己把他化为魔物，太自私 了。"),
                            addTaskTalk(1, "他没有死！他的魂魄还在！通天眼把夫君的魂魄 给了我，说我夫君的魂魄为 了我停留人间，不愿离开！ 那真是我夫君的魂魄啊，为 何你们都说是魔……为何！ ！"),
                            addTaskTalk(0, "通天眼给你的？难道那 九黎壶竟在通天眼手上？( 去寻找 通天眼 吧！)"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "你果然是通天眼！"),
                            addTaskTalk(1, "啧啧啧，别一脸怒容。他们本就不该在一起， 我多给了他们相处的一段日子，他们该感谢我呢！"));
                        break;
                    }
                case "3226":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "为什么要说谎？"),
                            addTaskTalk(1, "哈哈！我只是想证明给那位仙人看，我能决定命运！能掌握自己的生死，也能影响他人生死！命运在天？哈！狗屁！就像现在，我也能让你死在这些魔物爪 子下！（魔物聚集而来，先除掉魔 物吧！）"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "我就知道你不简单，没白浪费我口舌~"));
                        break;
                    }
                case "3227":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "命运命运！我通天眼知晓世间所有事的命途， 偏生不相信这命运！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你果然是被选中的 人，如果是你……应该能阻 止这一切吧……双月重天之 日即将到来，届时魔界之门 将会打开，去阻止天星子吧 ……阻止毁灭……"));
                        break;
                    }
                case "3228":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "天星子……我与他相识几十年……他野心太大，才会被侵袭……"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你竟然能安全出来，看来不是普通人啊。"));
                        break;
                    }
                //精英4
                case "3229":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "何为人？何为魔？魔 可化人，人可成魔。人为何 惧魔？魔为何厌人？万物皆 排除异己，何不相克相生？ 凡人，进去吧，魔与人皆有 选择权利，天下何归，皆看你们选择。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "嘿嘿嘿！ "),
                            addTaskTalk(0, "你怎么 会在这里？（摆出战斗架势 ）心魔：别这么粗鲁，我不 是来和你打架的！"));
                        break;
                    }
                case "3230":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "你不是想进里面吗？告诉你，你身上没有一丝魔气，进去会被万魔围攻滴！去击杀冥罗妖，取他们身上的彼岸花来，我能掩盖你的人气！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "（割下自己的血，滴入彼岸花中）佩戴在身上能掩盖凡人气息。"),
                            addTaskTalk(0, "我可以问你一个问题吗？"),
                            addTaskTalk(1, "爱过。"),
                            addTaskTalk(0, "......"));
                        break;
                    }
                case "3231":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "你想问我为什么帮你 吧？我只有在魔界才能具象 化，若魔军来到人界，我这 种在人界没有形体的拿什么 混吃？嘿嘿，我不过是不想 有人跟我抢地盘。"),
                            addTaskTalk(0, "......我该不该以绝后患先把你宰了呢......"),
                            addTaskTalk(1, "你还是先想想怎么宰了蜃兽吧！你能瞒得了众魔，却瞒不过 蜃兽 的鼻子！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "又一个凡人进来，当这里是旅游胜地么？"));
                        break;
                    }
                case "3232":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "不管怎样，意思意思也得先和你打一架！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "（打哈欠）进去吧，别吵到我睡觉。"),
                            addTaskTalk(0, "你......"));
                        break;
                    }
                case "3233":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(0, "那个啊，你知道天星子在哪里吗？"),
                            addTaskTalk(1, "另一个凡人？去了炼魂祭坛 了，那边的 熔骨尸煞不管人还是魔都照吃不误，不想死就别过去了。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "......"));
                        break;
                    }
                case "3234":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "以血为祭，滴落 祭坛）魔族之尊，我以血肉 之躯作祭奠，恳求魔尊降临 凡间！（一团黑雾环绕天星子，天 星子魔化了！）"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "为什么？为什么会 失败……他明明说过，命运 注定，天下将毁于一夕……壶中境 副本开启。至隐 之境找心魔可进入壶中境副 本。"));
                        break;
                    }
                //隐藏
                case "3235":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "冥冥天意，早有注 定……"),
                              addTaskTalk(0, "谁在说话？"),
                              addTaskTalk(1, "取出仙云环绕周围的 壶）那位仙人 在叫你， 进去 里面 找他吧。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "吼吼吼……就是你？就是你阻止了大人的计划！我要吃了你！"));
                        break;
                    }
                case "3236":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "吼吼！我要吃了你！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "......几百年没活动过了，果然身手不如从前了......"));
                        break;
                    }
                case "3237":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "我这才是第一关，去幻之境 ，只有收集到三份 天玄之证 证明你是命运之人，才能找到真正的 铸机 ！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "从来没有凡人能从我这里过去，你也不例外！"));
                        break;
                    }
                case "3238":
                    {
                        addArr(taskDes, "get",
                              addTaskTalk(1, "让我们把你变成灰烬吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哈哈哈，好久没打得这么痛快了！"));
                        break;
                    }
                case "3239":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "筋骨活动完，神清气爽！去吧，大人在等你！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "你就是他们说的那位仙人？"),
                            addTaskTalk(1, "孩子，你可知你的罪过？"),
                            addTaskTalk(0, "罪过？我有何过？"),
                            addTaskTalk(1, "本仙洞悉天机， 世间本应天命，循天道而行 。奈何紫薇七星异动，世间 竟脱离原有秩序……"));
                        break;
                    }
                case "3240":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "刘邦为天定君主 ，本该亡秦立汉，项羽虽为 西楚霸王，却应兵败刘邦而 自刎于乌江。奈何天道变幻 ，你竟逆天改命，使双方一 同抗秦，项羽逃过死劫，改变命运……数十年前我就察 觉到人间将有变化，于是将 天星子收归旗下。"),
                            addTaskTalk(0, "就算如此，你又为何要让魔军侵袭人界？"),
                            addTaskTalk(1, "天道变化，不能回归正道，只有毁了重建！"),
                            addTaskTalk(0, "放屁！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(0, "既然紫微星异动，那异动之后的才是天命！天命本就变幻无穷，你怎知你死守的天命是天命，我改变后的不是正确的天命？若你死守所谓的天道，入魔的便会是你！"),
                            addTaskTalk(1, "（轻捻胡须思考）年轻人，若你早出生几百年，兴许我们能在人间辨道，那便是十分有趣。"),
                            addTaskTalk(0, "人间？你不是仙人吗？"), 
                            addTaskTalk(1, "成仙前我在人间教书，世人称我为老子，有机会我们再畅谈一番，我已经解除赤炼洞窟中之石化，如今你先回去吧，我要思考一番。"));
                        break;
                    }
                case "3241":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "结束亦是开始......"));
                        addArr(taskDes, "p0",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "p1",
                            addTaskTalk(0, "......"));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "离开这里吧......"));
                        break;
                    }


                //震天
                case "3250":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "少侠，能在此处相遇也算有缘，可听老朽讲个故事？"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "上古传说，盘古破混沌，创世界。但混沌却并未完全消散，他们集聚成 强大的魔物震天战神。百年前，三大门派集众多高手， 将战神封印。"));
                        break;
                    }
                case "3251":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "可叹多年战乱，死伤无数，怨气冲天，封印震天战神的力量日渐衰弱，已然岌岌可危。我在此地等待可以重新封印战神的人出现，但是你要先战胜三大门派的六大神兽，证明自己的能力。现今三大门派设下神兽挑战台，你可先去墨家门派挑战遁甲堂主 ，他座下的神兽可是如坚壁一般强硬的家伙。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你可是前来挑战我座下的神兽玄甲神卫？"));
                        break;
                    }
                case "3252":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "本堂神兽 玄甲神卫身坚如盾，体硬如壁，你若击败得了他，本堂便承认你的实力，迎你为座上贵宾！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "玄甲神卫竟然败了？！"));
                        break;
                    }
                case "3253":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "你确非等闲之辈，只是你闯得了本堂，未必 能闯过其他分堂！去吧，道 家门派 的 琴魔堂主 正奏乐等 候强者前去挑战！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你可是前来挑战我座下的神兽落音仙灵？"));
                        break;
                    }
                case "3254":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "本堂神兽 落音仙灵身法轻盈，法力高强，你 若击败得了她，本堂便承认 你的实力，迎你为座上贵宾 ！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "落音仙灵竟然败于你手下！"));
                        break;
                    }
                case "3255":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "我本该察觉，你 相貌不凡，必非凡人。也许 你能击败 阴阳门派 剑中神兽 幽冥剑灵也并不见怪。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你可是前来挑战我座下的神兽幽冥剑灵？"));
                        break;
                    }
                case "3256":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "本堂神兽 幽冥剑灵为剑中之灵，剑气凶煞，凡人连三招都不能敌过，你若击败得了他，本堂便承认你的实力，迎你为座上贵宾！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, " 你......你竟然击败了幽冥剑灵？怎么可能！"));
                        break;
                    }
                case "3257":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "冥冥之中或有天定，去吧，去墨家门派找猛士堂主，也许你就是命中之人！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "你可是前来挑战我座下的神兽破天剑客？"));
                        break;
                    }
                case "3258":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "哈哈哈！笑话， 区区一个黄毛小孩，怎能敌 过破天剑客的剑气？听着， 小孩儿，你若击败得了 破天 剑客，本堂便承认你的实力 ，迎你为座上贵宾！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, " 这......这不可能！"));
                        break;
                    }
                case "3259":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "看来是英雄出少年啊！我不认老也不行了。许久未听 道家门派 的天音堂主美妙的乐声了，若前去 天音分堂 ，替我问候他吧。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, " 我已知晓，天命之人将现于世，那人......会是你吗？"));
                        break;
                    }
                case "3260":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "本堂神兽玉弦仙灵仙气萦绕，凡夫俗子不可近身，如果你是天命之人，那就让 玉弦仙灵 承认你吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, " 呵呵，果然......你就是天命之人！"));
                        break;
                    }
                case "3261":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "观你命格不凡，此行必有收获，去阴阳门派找罗刹堂主吧，一切都将结束！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "就是你战胜了五大神兽？"));
                        break;
                    }
                case "3262":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "非亲眼所见，恕我不能尽信。若想证明你实力，便击败本堂神兽玄冥紫魂吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "镇守震天战神的 六大神兽皆已负伤，施与震 天战神身上的封印力量渐趋 减弱。时隔百年，震天战神 将脱逃出封印，再次现于人 世！"),
                             addTaskTalk(0, "神秘？六大神兽竟然是镇守震天战神的神兽？可那老朽说......"),
                             addTaskTalk(1, "不必惊慌，你命格不凡，武近灭世，百年难见，为天之所定，命中注定你是再次封印震天战神的关键之人！"));
                        break;
                    }
                case "3263":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "近日有神秘人偷 取封印震天战神的封神壶， 此次三大门派设下神兽挑战 台，正是借机引出神秘人， 并甄选天下有能之人。那 神秘人必定正在解除震天战神的封印，趁封印刚解除，震天战神只有六成力量，去再次把它封印吧！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哈哈哈哈……封印即将解除，天下将陷入大乱！这些宝物是对你毁灭世界的奖赏，好好享受剩下的时光吧，哈哈！（老朽身上突然散发出黑色的邪气）"));
                        break;
                    }
                case "3264":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "（老朽口中念念 有词，他手中的封神壶壶口 冒出一团黑气，黑气渐渐聚 集成一个巨大的身影）哈哈 ！出来了！出来了！"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "哈哈哈哈……封印即将解除，天下将陷入大乱！这些宝物是对你毁灭世界的奖赏，好好享受剩下的时光吧，哈哈！（老朽身上突然散发出黑色的邪气）"));
                        break;
                    }
                    //魔神日常
                case "3265":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "最近流言四起，信徒 们都说有祭司在发贡香，我 坚信是没错的，一定是 狂攻祭司 获得了不少贡香，我要 你去一探究竟，估计 贡香 在 狂攻之护卫 身上，事成之后，必送厚礼。护卫不容易对付，建议组队。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然没有，看来是我想多了，好吧，作为回报今天得到唯一的一个供香就给你吧！"));
                        break;
                    }
                case "3266":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "最近流言四起，信徒 们都说有祭司在发贡香，我 坚信是没错的，一定是 铁壁祭司 获得了不少贡香，我要 你去一探究竟，估计 贡香 在 铁壁之护卫 身上，事成之后，必送厚礼。护卫不容易对付，建议组队。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然没有，看来是我想多了，好吧，作为回报今天得到唯一的一个供香就给你吧！"));
                        break;
                    }
                case "3267":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "最近流言四起，信徒 们都说有祭司在发贡香，我 坚信是没错的，一定是 生命祭司 获得了不少贡香，我要 你去一探究竟，估计 贡香 在 生命之护卫 身上，事成之后，必送厚礼。护卫不容易对付，建议组队。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然没有，看来是我想多了，好吧，作为回报今天得到唯一的一个供香就给你吧！"));
                        break;
                    }
                case "3268":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "最近流言四起，信徒 们都说有祭司在发贡香，我 坚信是没错的，一定是 神速祭司 获得了不少贡香，我要 你去一探究竟，估计 贡香 在 神速之护卫 身上，事成之后，必送厚礼。护卫不容易对付，建议组队。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然没有，看来是我想多了，好吧，作为回报今天得到唯一的一个供香就给你吧！"));
                        break;
                    }
                case "3269":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "最近流言四起，信徒 们都说有祭司在发贡香，我 坚信是没错的，一定是 射手祭司 获得了不少贡香，我要 你去一探究竟，估计 贡香 在 射手之护卫 身上，事成之后，必送厚礼。护卫不容易对付，建议组队。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然没有，看来是我想多了，好吧，作为回报今天得到唯一的一个供香就给你吧！"));
                        break;
                    }
                case "3270":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "最近流言四起，信徒 们都说有祭司在发贡香，我 坚信是没错的，一定是 法术祭司 获得了不少贡香，我要 你去一探究竟，估计 贡香 在 法术之护卫 身上，事成之后，必送厚礼。护卫不容易对付，建议组队。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然没有，看来是我想多了，好吧，作为回报今天得到唯一的一个供香就给你吧！"));
                        break;
                    }
                case "3271":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "最近流言四起，信徒 们都说有祭司在发贡香，我 坚信是没错的，一定是 暴怒祭司 获得了不少贡香，我要 你去一探究竟，估计 贡香 在 暴怒之护卫 身上，事成之后，必送厚礼。护卫不容易对付，建议组队。"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "居然没有，看来是我想多了，好吧，作为回报今天得到唯一的一个供香就给你吧！"));
                        break;
                    }

                case "3275":
                    {
                        addArr(taskDes, "get",
                            addTaskTalk(1, "梦境守护者：梦如凡世，却非凡世，世人总好梦幻而怯现实，污浊的身体，肮脏的灵魂，自私已成邪气，侵蚀梦幻古境，侠士请将这里的妖魔清理掉，还此处一片宁静吧......"));
                        addArr(taskDes, "p0",
                            addTaskTalk(1, "..."));
                        addArr(taskDes, "over",
                            addTaskTalk(1, "干得漂亮！"));
                        break;
                    }

                default:
                    {
                        *//* addArr(taskDes, "get",
                             addTaskTalk(1, "少侠，看你一表人才，没想到。。。"));
                         addArr(taskDes, "p0",
                             addTaskTalk(0, "xxx@**......"));
                         addArr(taskDes, "p1",
                             addTaskTalk(0, "1xxx@**......"));
                         addArr(taskDes, "p2",
                             addTaskTalk(0, "2xxx@**......"));
                         addArr(taskDes, "over",
                             addTaskTalk(1, "啊。。。"));
                         break;*//*
                        return null;
                    }
            }
            return taskDes;

        }*/
        private void addArr(JObject taskDes, string key, params JObject[] arr)
        {
            taskDes.Add(key, new JArray(arr));
        }
        private JObject addTaskTalk(int type, string talk)
        {
            JObject a = new JObject();
            a.Add("type", type);
            a.Add("talk", talk);
            return a;
        }
    }
    class TaskTalk
    {
        //0自己 1npc
        public int type;
        public string talk;

        public TaskTalk(int type, string talk)
        {
            this.type = type;
            this.talk = talk;
        }
    }
}
