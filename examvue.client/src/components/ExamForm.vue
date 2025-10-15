<template>
  <div class="max-w-xl mx-auto p-6 bg-white rounded-2xl shadow-lg mt-10">

    <div class="mb-4">
      <label class="block font-semibold mb-2">ชื่อ-สกุล</label>
      <input v-model="name" type="text" class="w-full border rounded px-3 py-2" />
    </div>

    <div class="mb-4">
      <p class="font-semibold">1. ข้อใดต่างจากข้ออื่น</p>
      <div v-for="opt in q1Options" :key="opt">
        <input type="radio" v-model="answers.q1" :value="opt" /> {{ opt }}
      </div>
    </div>

    <div class="mb-4">
      <p class="font-semibold">2. X + 2 = 4 จงหาค่า X</p>
      <div v-for="opt in q2Options" :key="opt">
        <input type="radio" v-model="answers.q2" :value="opt" /> {{ opt }}
      </div>
    </div>

    <div class="flex gap-4 mt-6">
      <button v-if="!submitted" @click="submitExam" class="bg-blue-500 text-white px-4 py-2 rounded">ส่งข้อสอบ</button>
      <button v-if="submitted" @click="redoExam" class="bg-gray-500 text-white px-4 py-2 rounded">ทำใหม่</button>
    </div>

    <div v-if="submitted" class="mt-6 text-green-700 font-bold">{{ resultMessage }}</div>
  </div>
</template>

<script setup>
import { ref } from 'vue';
import axios from 'axios';

const name = ref('');
const answers = ref({ q1: '', q2: '' });
const submitted = ref(false);
const resultMessage = ref('');

const q1Options = ["3","5","9","11"];
const q2Options = ["1","2","3","4"];
const correctAnswers = { q1: "9", q2: "2" };

async function submitExam() {
  if(!name.value){ alert("กรุณากรอกชื่อ"); return; }
  let score=0;
  if(answers.value.q1===correctAnswers.q1) score++;
  if(answers.value.q2===correctAnswers.q2) score++;

  try {
    const response = await fetch("/api/Exam/submit", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name: name.value, score })
    });

    if (!response.ok) throw new Error("HTTP error " + response.status);

    const data = await response.json();
    resultMessage.value = `คุณ ${data.name} สอบได้คะแนน: ${data.score}/2`;
    submitted.value = true;
  } catch (e) {
    console.error(e);
    alert("ส่งข้อมูลไม่สำเร็จ");
  }
}

function redoExam(){
  name.value=''; answers.value={q1:'',q2:''}; resultMessage.value=''; submitted.value=false;
}
</script>

<style>
  body {
    font-family: Segoe UI,Tahoma,Verdana,sans-serif;
    background: #f4f4f4;
  }
</style>
