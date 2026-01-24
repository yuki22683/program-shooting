#!/usr/bin/env python
# -*- coding: utf-8 -*-
"""Add InitializeTypeScriptLessons method to LessonManager.cs"""

typescript_method = '''
    /// <summary>
    /// Initialize TypeScript lessons from senkou-code data
    /// </summary>
    private void InitializeTypeScriptLessons()
    {
        lessons.Clear();

        // ==================== LESSON 1: TypeScript I ====================
        var lesson1 = new Lesson { titleKey = "typescript_lesson1_title" };

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex1_title",
            slideKeyPrefix = "typescript_lesson1_ex1",
            slideCount = 3,
            correctLines = new List<string> { "const message: string = 'Hello TS';", "console.log(message);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello TS" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex2_title",
            slideKeyPrefix = "typescript_lesson1_ex2",
            slideCount = 2,
            correctLines = new List<string> { "const x: number = 10;", "const y: number = 5;", "console.log(x + y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex2_comment1" }
            },
            expectedOutput = new List<string> { "15" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex3_title",
            slideKeyPrefix = "typescript_lesson1_ex3",
            slideCount = 2,
            correctLines = new List<string> { "console.log(10 % 3);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex3_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex4_title",
            slideKeyPrefix = "typescript_lesson1_ex4",
            slideCount = 2,
            correctLines = new List<string> { "let score: number = 50;", "score += 10;", "console.log(score);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex4_comment1" }
            },
            expectedOutput = new List<string> { "60" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex5_title",
            slideKeyPrefix = "typescript_lesson1_ex5",
            slideCount = 2,
            correctLines = new List<string> { "const age: number = 10;", "console.log(`私は${age}歳です`);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex5_comment1" }
            },
            expectedOutput = new List<string> { "私は10歳です" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex6_title",
            slideKeyPrefix = "typescript_lesson1_ex6",
            slideCount = 2,
            correctLines = new List<string> { "const colors: string[] = ['あか', 'あお'];", "console.log(colors[1]);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex6_comment1" }
            },
            expectedOutput = new List<string> { "あお" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex7_title",
            slideKeyPrefix = "typescript_lesson1_ex7",
            slideCount = 2,
            correctLines = new List<string> { "const isAdult: boolean = true;", "if (isAdult) {", "    console.log('おとなです');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex7_comment1" }
            },
            expectedOutput = new List<string> { "おとなです" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex8_title",
            slideKeyPrefix = "typescript_lesson1_ex8",
            slideCount = 2,
            correctLines = new List<string> { "const score: number = 75;", "if (score >= 80) {", "    console.log('ごうかく');", "} else {", "    console.log('ざんねん');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex8_comment1" }
            },
            expectedOutput = new List<string> { "ざんねん" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex9_title",
            slideKeyPrefix = "typescript_lesson1_ex9",
            slideCount = 2,
            correctLines = new List<string> { "const score: number = 85;", "if (score >= 80 && score <= 100) {", "    console.log('ごうかく');", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex9_comment1" }
            },
            expectedOutput = new List<string> { "ごうかく" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex10_title",
            slideKeyPrefix = "typescript_lesson1_ex10",
            slideCount = 3,
            correctLines = new List<string> { "const names: string[] = ['たろう', 'はなこ'];", "for (const name of names) {", "    console.log(name);", "}" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex10_comment1" }
            },
            expectedOutput = new List<string> { "たろう", "はなこ" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex11_title",
            slideKeyPrefix = "typescript_lesson1_ex11",
            slideCount = 2,
            correctLines = new List<string> { "type User = { name: string };", "const user: User = { name: 'たろう' };", "console.log(user.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex11_comment1" }
            },
            expectedOutput = new List<string> { "たろう" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex12_title",
            slideKeyPrefix = "typescript_lesson1_ex12",
            slideCount = 2,
            correctLines = new List<string> { "function greet(name: string) {", "    console.log(`こんにちは、${name}`);", "}", "greet('TypeScript');" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex12_comment1" }
            },
            expectedOutput = new List<string> { "こんにちは、TypeScript" }
        });

        lesson1.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson1_ex13_title",
            slideKeyPrefix = "typescript_lesson1_ex13",
            slideCount = 2,
            correctLines = new List<string> { "function showDate(): void {", "    console.log('今日の日付');", "}", "showDate();" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson1_ex13_comment1" }
            },
            expectedOutput = new List<string> { "今日の日付" }
        });

        lessons.Add(lesson1);

        // ==================== LESSON 2: TypeScript II ====================
        var lesson2 = new Lesson { titleKey = "typescript_lesson2_title" };

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex1_title",
            slideKeyPrefix = "typescript_lesson2_ex1",
            slideCount = 2,
            correctLines = new List<string> { "function show(value: string | number): void {", "    console.log(value);", "}", "show('Hello');", "show(42);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex1_comment1" }
            },
            expectedOutput = new List<string> { "Hello", "42" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex2_title",
            slideKeyPrefix = "typescript_lesson2_ex2",
            slideCount = 2,
            correctLines = new List<string> { "interface Person {", "    name: string;", "    age: number;", "}", "const p: Person = { name: 'Alice', age: 25 };", "console.log(p.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex2_comment1" }
            },
            expectedOutput = new List<string> { "Alice" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex3_title",
            slideKeyPrefix = "typescript_lesson2_ex3",
            slideCount = 2,
            correctLines = new List<string> { "interface Profile {", "    name: string;", "    nickname?: string;", "}", "const prof: Profile = { name: 'Bob' };", "console.log(prof.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex3_comment1" }
            },
            expectedOutput = new List<string> { "Bob" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex4_title",
            slideKeyPrefix = "typescript_lesson2_ex4",
            slideCount = 2,
            correctLines = new List<string> { "type Score = number;", "const math: Score = 85;", "const english: Score = 90;", "console.log(math + english);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex4_comment1" }
            },
            expectedOutput = new List<string> { "175" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex5_title",
            slideKeyPrefix = "typescript_lesson2_ex5",
            slideCount = 2,
            correctLines = new List<string> { "interface Item {", "    readonly id: number;", "    name: string;", "}", "const item: Item = { id: 1, name: 'Apple' };", "console.log(item.id);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex5_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex6_title",
            slideKeyPrefix = "typescript_lesson2_ex6",
            slideCount = 2,
            correctLines = new List<string> { "enum Day {", "    Sun,", "    Mon,", "    Tue", "}", "const today: Day = Day.Mon;", "console.log(today);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex6_comment1" }
            },
            expectedOutput = new List<string> { "1" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex7_title",
            slideKeyPrefix = "typescript_lesson2_ex7",
            slideCount = 2,
            correctLines = new List<string> { "function wrap<T>(value: T): T[] {", "    return [value];", "}", "const arr = wrap(5);", "console.log(arr);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex7_comment1" }
            },
            expectedOutput = new List<string> { "[ 5 ]" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex8_title",
            slideKeyPrefix = "typescript_lesson2_ex8",
            slideCount = 2,
            correctLines = new List<string> { "interface Container<T> {", "    item: T;", "}", "const box: Container<string> = { item: 'Hello' };", "console.log(box.item);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex8_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex9_title",
            slideKeyPrefix = "typescript_lesson2_ex9",
            slideCount = 2,
            correctLines = new List<string> { "const point = { x: 10, y: 20 };", "const point2: typeof point = { x: 5, y: 15 };", "console.log(point2.x);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex9_comment1" }
            },
            expectedOutput = new List<string> { "5" }
        });

        lesson2.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson2_ex10_title",
            slideKeyPrefix = "typescript_lesson2_ex10",
            slideCount = 2,
            correctLines = new List<string> { "interface Base {", "    id: number;", "}", "interface User extends Base {", "    name: string;", "}", "const u: User = { id: 1, name: 'Taro' };", "console.log(u.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson2_ex10_comment1" }
            },
            expectedOutput = new List<string> { "Taro" }
        });

        lessons.Add(lesson2);

        // ==================== LESSON 3: TypeScript III ====================
        var lesson3 = new Lesson { titleKey = "typescript_lesson3_title" };

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex1_title",
            slideKeyPrefix = "typescript_lesson3_ex1",
            slideCount = 2,
            correctLines = new List<string> { "interface Config { host: string; port: number; }", "function update(config: Config, patch: Partial<Config>): Config {", "  return { ...config, ...patch };", "}", "const cfg = { host: 'localhost', port: 3000 };", "console.log(update(cfg, { port: 8080 }).port);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex1_comment1" }
            },
            expectedOutput = new List<string> { "8080" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex2_title",
            slideKeyPrefix = "typescript_lesson3_ex2",
            slideCount = 2,
            correctLines = new List<string> { "interface Options { debug?: boolean; verbose?: boolean; }", "function init(opts: Required<Options>) {", "  console.log(opts.debug);", "}", "init({ debug: true, verbose: false });" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex2_comment1" }
            },
            expectedOutput = new List<string> { "true" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex3_title",
            slideKeyPrefix = "typescript_lesson3_ex3",
            slideCount = 2,
            correctLines = new List<string> { "interface Product { id: number; name: string; price: number; }", "type ProductName = Pick<Product, 'name'>;", "const item: ProductName = { name: 'Apple' };", "console.log(item.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex3_comment1" }
            },
            expectedOutput = new List<string> { "Apple" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex4_title",
            slideKeyPrefix = "typescript_lesson3_ex4",
            slideCount = 2,
            correctLines = new List<string> { "interface User { id: number; name: string; secret: string; }", "type SafeUser = Omit<User, 'secret'>;", "const user: SafeUser = { id: 1, name: 'Alice' };", "console.log(user.name);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex4_comment1" }
            },
            expectedOutput = new List<string> { "Alice" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex5_title",
            slideKeyPrefix = "typescript_lesson3_ex5",
            slideCount = 2,
            correctLines = new List<string> { "type Fruit = 'apple' | 'banana';", "type Prices = Record<Fruit, number>;", "const prices: Prices = { apple: 100, banana: 80 };", "console.log(prices.apple);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex5_comment1" }
            },
            expectedOutput = new List<string> { "100" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex6_title",
            slideKeyPrefix = "typescript_lesson3_ex6",
            slideCount = 2,
            correctLines = new List<string> { "function createPoint() { return { x: 10, y: 20 }; }", "type Point = ReturnType<typeof createPoint>;", "const p: Point = { x: 5, y: 15 };", "console.log(p.x + p.y);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex6_comment1" }
            },
            expectedOutput = new List<string> { "20" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex7_title",
            slideKeyPrefix = "typescript_lesson3_ex7",
            slideCount = 2,
            correctLines = new List<string> { "type IsArray<T> = T extends any[] ? true : false;", "type A = IsArray<number[]>;", "type B = IsArray<string>;", "const a: A = true;", "const b: B = false;", "console.log(a, b);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex7_comment1" }
            },
            expectedOutput = new List<string> { "true false" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex8_title",
            slideKeyPrefix = "typescript_lesson3_ex8",
            slideCount = 2,
            correctLines = new List<string> { "type Unwrap<T> = T extends Promise<infer U> ? U : T;", "type A = Unwrap<Promise<string>>;", "type B = Unwrap<number>;", "const a: A = 'hello';", "const b: B = 42;", "console.log(a, b);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex8_comment1" }
            },
            expectedOutput = new List<string> { "hello 42" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex9_title",
            slideKeyPrefix = "typescript_lesson3_ex9",
            slideCount = 2,
            correctLines = new List<string> { "interface Person { name: string; age: number; }", "function getProperty<K extends keyof Person>(p: Person, key: K) { return p[key]; }", "const person = { name: 'Bob', age: 30 };", "console.log(getProperty(person, 'name'));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex9_comment1" }
            },
            expectedOutput = new List<string> { "Bob" }
        });

        lesson3.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson3_ex10_title",
            slideKeyPrefix = "typescript_lesson3_ex10",
            slideCount = 2,
            correctLines = new List<string> { "type Optional<T> = { [K in keyof T]?: T[K]; };", "interface Config { host: string; port: number; }", "const partial: Optional<Config> = { host: 'localhost' };", "console.log(partial.host);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson3_ex10_comment1" }
            },
            expectedOutput = new List<string> { "localhost" }
        });

        lessons.Add(lesson3);

        // ==================== LESSON 4: TypeScript IV ====================
        var lesson4 = new Lesson { titleKey = "typescript_lesson4_title" };

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex1_title",
            slideKeyPrefix = "typescript_lesson4_ex1",
            slideCount = 2,
            correctLines = new List<string> { "function isNumber(x: unknown): x is number {", "  return typeof x === 'number';", "}", "const value: unknown = 42;", "if (isNumber(value)) { console.log(value * 2); }" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex1_comment1" }
            },
            expectedOutput = new List<string> { "84" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex2_title",
            slideKeyPrefix = "typescript_lesson4_ex2",
            slideCount = 2,
            correctLines = new List<string> { "type Car = { drive: () => void };", "type Boat = { sail: () => void };", "function operate(vehicle: Car | Boat): void {", "  if ('drive' in vehicle) { console.log('Driving'); } else { console.log('Sailing'); }", "}", "operate({ drive: () => {} });" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex2_comment1" }
            },
            expectedOutput = new List<string> { "Driving" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex3_title",
            slideKeyPrefix = "typescript_lesson4_ex3",
            slideCount = 2,
            correctLines = new List<string> { "type Success = { status: 'success'; data: string };", "type Failure = { status: 'failure'; error: string };", "type Result = Success | Failure;", "function handle(result: Result): void {", "  switch (result.status) { case 'success': console.log(result.data); break; case 'failure': console.log(result.error); break; }", "}", "handle({ status: 'success', data: 'OK' });" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex3_comment1" }
            },
            expectedOutput = new List<string> { "OK" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex4_title",
            slideKeyPrefix = "typescript_lesson4_ex4",
            slideCount = 2,
            correctLines = new List<string> { "type Color = 'red' | 'green' | 'blue';", "function getHex(color: Color): string {", "  switch (color) { case 'red': return '#ff0000'; case 'green': return '#00ff00'; case 'blue': return '#0000ff'; default: const _exhaustive: never = color; return _exhaustive; }", "}", "console.log(getHex('red'));" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex4_comment1" }
            },
            expectedOutput = new List<string> { "#ff0000" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex5_title",
            slideKeyPrefix = "typescript_lesson4_ex5",
            slideCount = 2,
            correctLines = new List<string> { "type Method = 'get' | 'post';", "type Endpoint = '/users' | '/posts';", "type Route = `${Method} ${Endpoint}`;", "const route: Route = 'get /users';", "console.log(route);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex5_comment1" }
            },
            expectedOutput = new List<string> { "get /users" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex6_title",
            slideKeyPrefix = "typescript_lesson4_ex6",
            slideCount = 2,
            correctLines = new List<string> { "type Status = 'pending' | 'success' | 'error' | 'cancelled';", "type ActiveStatus = Exclude<Status, 'cancelled'>;", "const status: ActiveStatus = 'pending';", "console.log(status);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex6_comment1" }
            },
            expectedOutput = new List<string> { "pending" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex7_title",
            slideKeyPrefix = "typescript_lesson4_ex7",
            slideCount = 2,
            correctLines = new List<string> { "type Event = 'click' | 'scroll' | 'mouseover' | 'keydown';", "type MouseEvent = Extract<Event, 'click' | 'scroll' | 'mouseover'>;", "const event: MouseEvent = 'click';", "console.log(event);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex7_comment1" }
            },
            expectedOutput = new List<string> { "click" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex8_title",
            slideKeyPrefix = "typescript_lesson4_ex8",
            slideCount = 2,
            correctLines = new List<string> { "type MaybeString = string | null | undefined;", "type DefiniteString = NonNullable<MaybeString>;", "const text: DefiniteString = 'Hello';", "console.log(text);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex8_comment1" }
            },
            expectedOutput = new List<string> { "Hello" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex9_title",
            slideKeyPrefix = "typescript_lesson4_ex9",
            slideCount = 2,
            correctLines = new List<string> { "function greet(name: string, age: number): void { console.log(`${name} is ${age}`); }", "type GreetParams = Parameters<typeof greet>;", "const args: GreetParams = ['Taro', 25];", "greet(...args);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex9_comment1" }
            },
            expectedOutput = new List<string> { "Taro is 25" }
        });

        lesson4.exercises.Add(new Exercise
        {
            titleKey = "typescript_lesson4_ex10_title",
            slideKeyPrefix = "typescript_lesson4_ex10",
            slideCount = 2,
            correctLines = new List<string> { "type AsyncResult = Promise<{ data: string }>;", "type Result = Awaited<AsyncResult>;", "const result: Result = { data: 'success' };", "console.log(result.data);" },
            comments = new List<LocalizedComment>
            {
                new LocalizedComment { lineIndex = 0, commentPrefix = "//", localizationKey = "typescript_lesson4_ex10_comment1" }
            },
            expectedOutput = new List<string> { "success" }
        });

        lessons.Add(lesson4);

        currentLessonIndex = 0;
        currentExerciseIndex = 0;

        Debug.Log($"[LessonManager] Initialized {lessons.Count} TypeScript lessons");
    }

'''

def main():
    # Read the LessonManager.cs file
    with open('Assets/Scripts/LessonManager.cs', 'r', encoding='utf-8') as f:
        content = f.read()

    # Find the position to insert (after InitializeJavaScriptLessons closing brace)
    target = '        Debug.Log($"[LessonManager] Initialized {lessons.Count} JavaScript lessons");\n    }\n'

    if target not in content:
        print("ERROR: Could not find insertion point")
        return 1

    # Insert the TypeScript method after the JavaScript method
    new_content = content.replace(target, target + typescript_method)

    # Write the modified content back
    with open('Assets/Scripts/LessonManager.cs', 'w', encoding='utf-8') as f:
        f.write(new_content)

    print("SUCCESS: TypeScript lessons method added")
    return 0

if __name__ == '__main__':
    exit(main())
